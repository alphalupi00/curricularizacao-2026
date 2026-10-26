using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace DULCIESBOLOS.Models
{
    [Table("Funcionario")]
    public class Funcionario
    {
        [Display(Name = "Id: ")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Nome: ")]
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Telefone: ")]
        [Required(ErrorMessage = "O campo telefone é obrigatório")]
        [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres")]
        public string Telefone { get; set; } = string.Empty;

        [Display(Name = "Horário de serviço: ")] 
        [Required(ErrorMessage = "O campo horário de serviço é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo horário de serviço deve ter no máximo 100 caracteres")]
        public string horarioServico { get; set; } = string.Empty;
    }
}
