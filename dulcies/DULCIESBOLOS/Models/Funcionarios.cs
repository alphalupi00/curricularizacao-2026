using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace DULCIESBOLOS.Models{

[Table("Funcionario")] //criando mapemaneto par melhorar a qualidade do projeto
public class Funcionario
{
   [Display(Name = "Id: ")]
   [Key]
   [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
   public int Id { get; set; }


   [Display(Name = "Nome: ")]
   [Required(ErrorMessage = "O campo nome é obrigatório")]
   [StringLength(35, ErrorMessage = "O campo nome deve ter no máximo 35 caracteres")]
   public string Nome { get; set; } = string.Empty;


   [Display(Name = "Telefone: ")]
   [Required(ErrorMessage = "O campo telefone é obrigatório")]
   [Range(1, int.MaxValue, ErrorMessage = "Digite um número de telefone válido")]
   public int Telefone { get; set; }


   [Display(Name = "Cargo: ")] 
   [Required(ErrorMessage = "O campo cargo é obrigatório")] 
   public int? cargoId { get; set; } 

   [Display(Name = "Cargo: ")]
   [ForeignKey("cargoId")]
   public virtual Cargo? Cargo { get; set; }


   [Display(Name = "Horário de serviço: ")] 
   [Required(ErrorMessage = "O campo horário de serviço é obrigatório")]
   [StringLength(100, ErrorMessage = "O campo horário de serviço deve ter no máximo 100 caracteres")]
   public string horarioServico { get; set; } = string.Empty;
}

}