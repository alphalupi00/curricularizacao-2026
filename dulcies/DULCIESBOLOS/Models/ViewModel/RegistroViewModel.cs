using System.ComponentModel.DataAnnotations;

namespace DULCIESBOLOS.Models.ViewModel
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "O campo nome completo é obrigatório.")]
        [Display(Name = "Nome Completo")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Telefone")]
        [Phone(ErrorMessage = "Informe um telefone válido.")]
        public string? Telefone { get; set; }

        [Display(Name = "Endereço")]
        [StringLength(150, ErrorMessage = "O endereço deve ter no máximo 150 caracteres.")]
        public string? Endereco { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Senha", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}
