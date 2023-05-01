using System.ComponentModel.DataAnnotations;

namespace APIVENDASCORE.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter exatamente 11 dígitos.")]
        public string Cpf { get; set; }

        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "A senha deve ter pelo menos 8 caracteres, incluindo pelo menos uma letra maiúscula, uma letra minúscula e um número.")]
        public string Senha { get; set; }
        public int Tipo { get; set; }
    }
}
