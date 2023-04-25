using System.ComponentModel.DataAnnotations;

namespace APIVENDASCORE.Models
{
    public class Vendedor
    {
        [Key]
        public int Id { get; set; }
        public string Cnpj { get; set; }
        public string Senha { get; set; }
        public int Tipo { get; set; }
    }
}
