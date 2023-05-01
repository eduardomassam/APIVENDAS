using System.ComponentModel.DataAnnotations;

namespace APIVENDASCORE.Models
{
    public class Vendedor
    {
        [Key]
        public int Id { get; set; }
        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CPF deve conter exatamente 14 dígitos.")]
        public string Cnpj { get; set; }
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "A senha deve ter pelo menos 8 caracteres, incluindo pelo menos uma letra maiúscula, uma letra minúscula e um número.")]

        public string Senha { get; set; }
        public int Tipo { get; set; }
    }
}
