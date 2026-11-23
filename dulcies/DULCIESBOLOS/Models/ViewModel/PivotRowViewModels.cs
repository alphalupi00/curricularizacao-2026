using System.Collections.Generic;

namespace DULCIESBOLOS.Models.ViewModel
{

    public class PivotRowViewModel
    {
        public string Sabor { get; set; } = string.Empty;
        public string Tamanho { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; } = 0;
        public Dictionary<string, string> VendasPorMes { get; set; } = new();
        public int TotalQuantidade { get; set; } = 0;
        public string TotalQuantidadeFormatada => TotalQuantidade > 0 ? $"{TotalQuantidade} Un" : "0 Un";
        public decimal TotalValorReais { get; set; } = 0;
        public string TotalValorFormatado => TotalValorReais.ToString("C2");
    }
}
