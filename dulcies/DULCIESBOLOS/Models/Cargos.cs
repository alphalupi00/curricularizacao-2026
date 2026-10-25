using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace DULCIESBOLOS.Models{

    [Table("Cargo")]  
    public class Cargo
    {
        [Display(Name = "Id: ")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Cargo: ")]
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [StringLength(35, ErrorMessage = "O campo cargo deve ter no máximo 35 caracteres")]
        public string nomeCargo { get; set; } = string.Empty;
    }
} 