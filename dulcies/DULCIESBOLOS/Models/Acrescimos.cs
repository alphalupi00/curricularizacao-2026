using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace DULCIESBOLOS.Models{

    [Table("Acrescimo")]  
    public class Acrescimo
    {
        [Display(Name = "Id: ")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Descrição: ")]
        [Required(ErrorMessage = "O campo descrição é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo descrição deve ter no máximo 100 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Display(Name = "Quantidade (em gramas): ")]
        [Required(ErrorMessage = "O campo quantidade é obrigatório")]
        [StringLength(5, ErrorMessage = "O campo quantidade deve ter no máximo 5 caracteres")]
        public string Quantidade { get; set; } = string.Empty;


       [Display(Name = "Valor (R$): ")]
       [Column(TypeName = "decimal(18,2)")]
       [Required(ErrorMessage = "O campo valor é obrigatório")]
       public decimal Valor { get; set; } = 0;

       [Display(Name = "Estoque: ")]
       [Required(ErrorMessage = "O campo estoque é obrigatório")]
       [Range(0, 9999, ErrorMessage = "O estoque não pode ser negativo")]
       public int Estoque { get; set; } = 20;
    }
} 