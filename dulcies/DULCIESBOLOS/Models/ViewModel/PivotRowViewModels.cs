using System.Collections.Generic;

namespace DULCIESBOLOS.Models.ViewModel
{
    /// <summary>
    /// Representa uma linha da tabela de Agrupamento Pivot.
    /// Pode ser um Cliente ou um Período/Mês, contendo as quantidades de cada sabor de bolo.
    /// </summary>
    public class PivotRowViewModel
    {
        public string Identificador { get; set; } = string.Empty;
        public string Detalhe { get; set; } = string.Empty;
        public Dictionary<int, string> ValoresPorSabor { get; set; } = new();
        public int TotalQuantidade { get; set; } = 0;
        public string TotalQuantidadeFormatada { get; set; } = string.Empty;
        public decimal TotalValorReais { get; set; } = 0;
        public string TotalValorFormatado => TotalValorReais.ToString("C2");
    }
}
