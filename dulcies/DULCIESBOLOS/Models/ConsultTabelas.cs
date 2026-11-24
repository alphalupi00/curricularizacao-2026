using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DULCIESBOLOS.Models
{
    public class ConsultTabelas
    {
        public string Sabor { get; set; } = string.Empty;

        public int QuantidadeVendida { get; set; }

        public decimal FaturamentoTotal { get; set; }
    }
}