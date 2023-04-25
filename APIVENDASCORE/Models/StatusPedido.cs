using System.ComponentModel.DataAnnotations;

namespace APIVENDASCORE.Models
{
    public class StatusPedido
    {
        [Key]
        public int Codigo { get; set; }
        public string Descricao { get; set; }
    }
}
