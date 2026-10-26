using System;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace DULCIESBOLOS.Models
{
    [Table("Venda")]  
    public class Venda
    {
        [Display(Name = "Id: ")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Usuário: ")] 
        public string UsuarioId { get; set; } 

        [Display(Name = "Cliente: ")]
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } 

        [Display(Name = "Bolo: ")] 
        [Required(ErrorMessage = "Informar o bolo é obrigatório")] 
        public int? boloId { get; set; } 

        [Display(Name = "Bolo: ")]
        [ForeignKey("boloId")]
        public virtual Bolo Bolo { get; set; }

        [Display(Name = "Quantidade: ")]
        [Required(ErrorMessage = "O campo quantidade é obrigatório")]
        [Range(1, 100, ErrorMessage = "A quantidade deve ser de no mínimo 1 unidade")]
        public int Quantidade { get; set; } = 1;

        [Display(Name = "Total (R$): ")]
        public decimal Valor { get; set; } = 0;

        [Display(Name = "Data do Pedido: ")]
        public DateTime DataVenda { get; set; } = DateTime.Now;

        [Display(Name = "Status: ")]
        [StringLength(30)]
        public string Status { get; set; } = "Pendente";

        [Display(Name = "Forma de pagamento: ")] 
        [Required(ErrorMessage = "O campo forma de pagamento é obrigatório")] 
        public int? formaPagamentoId { get; set; } 

        [Display(Name = "Forma de pagamento: ")]
        [ForeignKey("formaPagamentoId")]
        public virtual Pagamento Pagamento { get; set; }
    }
}
