using System.ComponentModel.DataAnnotations;

namespace Cliente.Models
{
    public class EsqueceuSenhaViewModel
    {
        [Required(ErrorMessage = "O campo CPF é obrigatório.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo Senha Atual é obrigatório.")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "A senha deve ter pelo menos 8 caracteres, incluindo pelo menos uma letra maiúscula, uma letra minúscula e um número.")]

        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "O campo Senha Nova é obrigatório.")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "A senha deve ter pelo menos 8 caracteres, incluindo pelo menos uma letra maiúscula, uma letra minúscula e um número.")]
        public string SenhaNova { get; set; }
    }
}