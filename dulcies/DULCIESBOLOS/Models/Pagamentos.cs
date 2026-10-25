using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace DULCIESBOLOS.Models{

    [Table("Pagamento")]  
    public class Pagamento
    {
        [Display(Name = "Id: ")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Descrição: ")]
        [Required(ErrorMessage = "O campo descrição é obrigatório")]
        [StringLength(35, ErrorMessage = "O campo descrição deve ter no máximo 35 caracteres")]
        public string descricao { get; set; } = string.Empty;
    }
} 