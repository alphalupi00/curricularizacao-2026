using System.Collections.Generic;
using DULCIESBOLOS.Models;

namespace DULCIESBOLOS.Models.ViewModel
{
    /// <summary>
    /// Modelo da Tabela Pivot de Vendas por Sabor de Bolo.
    /// Contém os sabores de bolos cadastrados como colunas, as linhas agrupadas
    /// e os totais gerais calculados.
    /// </summary>
    public class PivotBolosViewModel
    {
        public List<Bolo> Sabores { get; set; } = new();
        public List<PivotRowViewModel> Linhas { get; set; } = new();
        public Dictionary<int, string> TotaisPorSabor { get; set; } = new();
        public string TipoAgrupamento { get; set; } = "cliente";
        public int QuantidadeGeralTotal { get; set; } = 0;
        public string QuantidadeGeralTotalFormatada { get; set; } = string.Empty;
        public decimal ValorGeralTotal { get; set; } = 0;
        public string ValorGeralTotalFormatado => ValorGeralTotal.ToString("C2");
    }
}
