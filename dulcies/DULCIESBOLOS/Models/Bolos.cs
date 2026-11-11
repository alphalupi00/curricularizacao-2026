using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace DULCIESBOLOS.Models
{
    [Table("Bolo")]  
    public class Bolo
    {
        [Display(Name = "Id: ")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Sabor: ")]
        [Required(ErrorMessage = "O campo Sabor é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo Sabor deve ter no máximo 100 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Display(Name = "Tamanho (P, M ou G): ")]
        [Required(ErrorMessage = "O campo tamanho é obrigatório (informe P, M ou G)")]
        [RegularExpression("^(P|M|G|p|m|g)$", ErrorMessage = "O tamanho deve ser P, M ou G")]
        [StringLength(5, ErrorMessage = "O campo tamanho deve ter no máximo 5 caracteres")]
        public string Tamanho { get; set; } = "M";

        [Display(Name = "Valor (R$): ")]
        [Required(ErrorMessage = "O campo valor é obrigatório")]
        [Range(0.01, 9999.99, ErrorMessage = "O valor deve ser maior que zero")]
        public float Valor { get; set; } = 0;
    }
}
