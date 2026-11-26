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

    public class PivotService
    {
        private readonly AppDBContext _context;

        public PivotService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<PivotBolosViewModel> ObterPivotVendasPorSaborAsync()
        {

            var bolos = await _context.Bolo
                .OrderBy(b => b.Descricao)
                .ThenBy(b => b.Tamanho)
                .AsNoTracking()
                .ToListAsync();


            var vendas = await _context.Venda
                .AsNoTracking()
                .ToListAsync();


            var colunasMeses = vendas
                .GroupBy(v => new { v.DataVenda.Year, v.DataVenda.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g =>
                {
                    var data = new DateTime(g.Key.Year, g.Key.Month, 1);
                    var nome = data.ToString("MMMM / yyyy", new CultureInfo("pt-BR"));
                    nome = char.ToUpper(nome[0]) + nome.Substring(1);
                    return new ColunaMesViewModel
                    {
                        Ano = g.Key.Year,
                        Mes = g.Key.Month,
                        NomeMesFormatado = nome
                    };
                })
                .ToList();

            //tratamento caso nao haja vendas
            if (!colunasMeses.Any())
            {
                var agora = DateTime.Now;
                var nome = agora.ToString("MMMM / yyyy", new CultureInfo("pt-BR"));
                nome = char.ToUpper(nome[0]) + nome.Substring(1);
                colunasMeses.Add(new ColunaMesViewModel
                {
                    Ano = agora.Year,
                    Mes = agora.Month,
                    NomeMesFormatado = nome
                });
            }


            // PIVOT: Monta as linhas para cada sabor e tamanho
            var linhas = new List<PivotRowViewModel>();

            foreach (var bolo in bolos)
            {
                var linha = new PivotRowViewModel
                {
                    Sabor = bolo.Descricao,
                    Tamanho = bolo.Tamanho?.ToUpper() ?? "M",
                    PrecoUnitario = (decimal)bolo.Valor
                };

                var vendasDoBoloPorMes = vendas
                    .Where(v => v.boloId == bolo.Id)
                    .GroupBy(v => $"{v.DataVenda.Year}-{v.DataVenda.Month:D2}")
                    .ToDictionary(
                        g => g.Key,
                        g => new
                        {
                            Quantidade = g.Sum(v => v.Quantidade),
                            ValorTotal = g.Sum(v => v.Valor)
                        }
                    );
                
                foreach (var col in colunasMeses)
                {
                    if (vendasDoBoloPorMes.TryGetValue(col.Chave, out var dados) && dados.Quantidade > 0)
                    {
                        linha.VendasPorMes[col.Chave] = $"{dados.Quantidade} un";
                        linha.TotalQuantidade += dados.Quantidade;
                        linha.TotalValorReais += dados.ValorTotal;
                    }
                    else
                    {
                        linha.VendasPorMes[col.Chave] = "-";
                    }
                }

                linhas.Add(linha);
            }

            var viewModel = new PivotBolosViewModel
            {
                ColunasMeses = colunasMeses,
                Linhas = linhas,
                QuantidadeGeralTotal = vendas.Sum(v => v.Quantidade),
                ValorGeralTotal = vendas.Sum(v => v.Valor)
            };

            return viewModel;
        }
    }
}
