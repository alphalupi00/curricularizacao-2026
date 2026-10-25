using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace DULCIESBOLOS.Models{

    [Table("Venda")]  
    public class Venda
    {
        [Display(Name = "Id: ")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        [Display(Name = "Cliente: ")] 
        [Required(ErrorMessage = "O campo cliente é obrigatório")] 
        public int? clienteId { get; set; } 

        [Display(Name = "Cliente: ")]
        [ForeignKey("clienteId")]
        public virtual Cliente? Cliente { get; set; } 


        
        [Display(Name = "Funcionario: ")] 
        [Required(ErrorMessage = "O campo funcionário é obrigatório")] 
        public int? funcionarioId { get; set; } 

        [Display(Name = "Funcionario: ")]
        [ForeignKey("funcionarioId")]
        public virtual Funcionario? Funcionario { get; set; }



        [Display(Name = "Bolo: ")] 
        [Required(ErrorMessage = "Informar o bolo é obrigatório")] 
        public int? boloId { get; set; } 

        [Display(Name = "Bolo: ")]
        [ForeignKey("boloId")]
        public virtual Bolo? Bolo { get; set; }



        [Display(Name = "Quantidade: ")]
        [Required(ErrorMessage = "O campo quantidade é obrigatório")]
        public string Quantidade { get; set; } = string.Empty;


       [Display(Name = "Total: ")]
       [Required(ErrorMessage = "O campo total é obrigatório")]
       public float Valor { get; set; } = 0;



        [Display(Name = "Forma de pagamento: ")] 
        [Required(ErrorMessage = "O campo forma de pagamento é obrigatório")] 
        public int? formaPagamentoId { get; set; } 

        [Display(Name = "Forma de pagamento: ")]
        [ForeignKey("formaPagamentoId")]
        public virtual Pagamento? Pagamento { get; set; }
    }
} 