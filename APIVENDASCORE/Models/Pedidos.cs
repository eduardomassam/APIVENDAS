using System.ComponentModel.DataAnnotations;

namespace APIVENDASCORE.Models
{
    public class Pedidos
    {
        [Key]
        public int Cod { get; set; }
        public string CPF { get; set; }
        public string NomeCliente { get; set; }
        public string Produto { get; set; }
        public Nullable<int> Quantidade { get; set; }
        public int Status { get; set; }
    }
}
