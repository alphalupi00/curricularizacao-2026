using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace DULCIESBOLOS.Models{

    [Table("Bolo")]  
    public class Bolo
    {
        [Display(Name = "Id: ")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Descrição: ")]
        [Required(ErrorMessage = "O campo descrição é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo descrição deve ter no máximo 100 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Display(Name = "Tamanho: ")]
        [Required(ErrorMessage = "O campo tamanho é obrigatório")]
        [StringLength(5, ErrorMessage = "O campo tamanho deve ter no máximo 5 caracteres")]
        public string Tamanho { get; set; } = string.Empty;


       [Display(Name = "Valor: ")]
       [Required(ErrorMessage = "O campo valor é obrigatório")]
       public float Valor { get; set; } = 0;
    }
} 