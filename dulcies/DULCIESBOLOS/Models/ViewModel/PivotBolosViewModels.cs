using System.Collections.Generic;

namespace DULCIESBOLOS.Models.ViewModel
{

    public class ColunaMesViewModel
    {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public string Chave => $"{Ano}-{Mes:D2}";
        public string NomeMesFormatado { get; set; } = string.Empty;
    }

    public class PivotBolosViewModel
    {
        public List<ColunaMesViewModel> ColunasMeses { get; set; } = new();
        public List<PivotRowViewModel> Linhas { get; set; } = new();
        public int QuantidadeGeralTotal { get; set; } = 0;
        public string QuantidadeGeralTotalFormatada => $"{QuantidadeGeralTotal} unidade(s)";
        public decimal ValorGeralTotal { get; set; } = 0;
        public string ValorGeralTotalFormatado => ValorGeralTotal.ToString("C2");
    }
}
