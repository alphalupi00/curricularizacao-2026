#nullable enable
using System;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DULCIESBOLOS.Models
{
    [Table("Venda")]  
    public class Venda
    {
        [Display(Name = "Id: ")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Cliente: ")]
        [Required(ErrorMessage = "O campo Cliente é obrigatório")]
        public string UsuarioId { get; set; } = string.Empty;

        [ForeignKey("UsuarioId")]
        [ValidateNever]
        public virtual Usuario? Usuario { get; set; } 

        [Display(Name = "Bolo: ")] 
        [Required(ErrorMessage = "O campo Bolo é obrigatório")] 
        public int? boloId { get; set; }

        [ForeignKey("boloId")]
        [ValidateNever]
        public virtual Bolo? Bolo { get; set; }

        [Display(Name = "Quantidade: ")]
        [Required(ErrorMessage = "O campo quantidade é obrigatório")]
        [Range(1, 100, ErrorMessage = "A quantidade deve ser de no mínimo 1 unidade")]
        public int Quantidade { get; set; } = 1;

        [Display(Name = "Total (R$): ")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; } = 0;

        [Display(Name = "Data do Pedido: ")]
        public DateTime DataVenda { get; set; } = DateTime.Now;

        [Display(Name = "Status: ")]
        [StringLength(30)]
        public string Status { get; set; } = "Pendente";

        [Display(Name = "Forma de pagamento: ")] 
        [Required(ErrorMessage = "O campo forma de pagamento é obrigatório")] 
        public int? formaPagamentoId { get; set; } 

        [ForeignKey("formaPagamentoId")]
        [ValidateNever]
        public virtual Pagamento? Pagamento { get; set; }
    }
}
