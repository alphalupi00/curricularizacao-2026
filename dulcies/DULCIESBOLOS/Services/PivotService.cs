using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DULCIESBOLOS.Data;
using DULCIESBOLOS.Models;
using DULCIESBOLOS.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DULCIESBOLOS.Services
{
    public class PivotService : IPivotService
    {
        private readonly AppDBContext _context;

        public PivotService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<PivotBolosViewModel> ObterPivotVendasPorSaborAsync(string tipoAgrupamento = "cliente")
        {
            var sabores = await _context.Bolo
                .OrderBy(b => b.Descricao)
                .ThenBy(b => b.Tamanho)
                .AsNoTracking()
                .ToListAsync();

            var vendas = await _context.Venda
                .Include(v => v.Bolo)
                .Include(v => v.Usuario)
                .AsNoTracking()
                .ToListAsync();

            var viewModel = new PivotBolosViewModel
            {
                Sabores = sabores,
                TipoAgrupamento = string.Equals(tipoAgrupamento, "mes", StringComparison.OrdinalIgnoreCase) ? "mes" : "cliente"
            };

            var acumuladorPorSabor = sabores.ToDictionary(b => b.Id, _ => 0);

            if (viewModel.TipoAgrupamento == "mes")
            {
                var vendasPorMes = vendas
                    .GroupBy(v => new { v.DataVenda.Year, v.DataVenda.Month })
                    .OrderByDescending(g => g.Key.Year)
                    .ThenByDescending(g => g.Key.Month);

                foreach (var grupo in vendasPorMes)
                {
                    var dataReferencia = new DateTime(grupo.Key.Year, grupo.Key.Month, 1);
                    var nomeMes = dataReferencia.ToString("MMMM / yyyy", new CultureInfo("pt-BR"));
                    nomeMes = char.ToUpper(nomeMes[0]) + nomeMes.Substring(1);

                    var linha = MontarLinhaPivot(
                        identificador: nomeMes,
                        detalhe: $"Ano {grupo.Key.Year}",
                        vendasDoGrupo: grupo.ToList(),
                        sabores: sabores,
                        acumuladorPorSabor: acumuladorPorSabor
                    );

                    viewModel.Linhas.Add(linha);
                }
            }
            else
            {
                var vendasPorCliente = vendas
                    .GroupBy(v => v.UsuarioId ?? (v.Usuario != null ? v.Usuario.Id : "Balcao"))
                    .OrderBy(g => g.First().Usuario?.NomeCompleto ?? "Cliente Balcão");

                foreach (var grupo in vendasPorCliente)
                {
                    var primeiro = grupo.First();
                    string nomeCliente = primeiro.Usuario?.NomeCompleto ?? "Cliente Balcão";
                    string detalhe = !string.IsNullOrEmpty(primeiro.Usuario?.PhoneNumber) 
                        ? primeiro.Usuario.PhoneNumber 
                        : (primeiro.Usuario?.Email ?? "Sem contato");

                    var linha = MontarLinhaPivot(
                        identificador: nomeCliente,
                        detalhe: detalhe,
                        vendasDoGrupo: grupo.ToList(),
                        sabores: sabores,
                        acumuladorPorSabor: acumuladorPorSabor
                    );

                    viewModel.Linhas.Add(linha);
                }
            }

            foreach (var sabor in sabores)
            {
                int totalSabor = acumuladorPorSabor[sabor.Id];
                viewModel.TotaisPorSabor[sabor.Id] = totalSabor > 0 
                    ? $"{totalSabor} un" 
                    : "-";

                viewModel.QuantidadeGeralTotal += totalSabor;
            }

            viewModel.QuantidadeGeralTotalFormatada = $"{viewModel.QuantidadeGeralTotal} unidade(s)";
            viewModel.ValorGeralTotal = vendas.Sum(v => v.Valor);

            return viewModel;
        }

        private PivotRowViewModel MontarLinhaPivot(
            string identificador,
            string detalhe,
            List<Venda> vendasDoGrupo,
            List<Bolo> sabores,
            Dictionary<int, int> acumuladorPorSabor)
        {
            var linha = new PivotRowViewModel
            {
                Identificador = identificador,
                Detalhe = detalhe,
                TotalValorReais = vendasDoGrupo.Sum(v => v.Valor)
            };

            var vendasPorBolo = vendasDoGrupo
                .Where(v => v.boloId.HasValue)
                .GroupBy(v => v.boloId.Value)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(v => v.Quantidade)
                );

            foreach (var sabor in sabores)
            {
                if (vendasPorBolo.TryGetValue(sabor.Id, out int quantidadeVendida) && quantidadeVendida > 0)
                {
                    linha.ValoresPorSabor[sabor.Id] = $"{quantidadeVendida} un";
                    linha.TotalQuantidade += quantidadeVendida;
                    acumuladorPorSabor[sabor.Id] += quantidadeVendida;
                }
                else
                {
                    linha.ValoresPorSabor[sabor.Id] = "-";
                }
            }

            linha.TotalQuantidadeFormatada = linha.TotalQuantidade > 0 
                ? $"{linha.TotalQuantidade} un" 
                : "0 un";

            return linha;
        }
    }
}
