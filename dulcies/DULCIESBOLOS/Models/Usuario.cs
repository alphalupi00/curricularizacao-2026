using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DULCIESBOLOS.Models
{
    public class Usuario : IdentityUser
    {
        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O campo nome completo é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Display(Name = "Endereço")]
        [StringLength(150, ErrorMessage = "O endereço deve ter no máximo 150 caracteres")]
        public string Endereco { get; set; } = string.Empty;
    }
}

