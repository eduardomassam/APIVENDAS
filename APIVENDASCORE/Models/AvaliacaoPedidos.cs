using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace APIVENDASCORE.Models
{
    public class AvaliacaoPedidos
    {
        [Key]
        public int CodPedido { get; set; }
        public string Avaliacao { get; set; }
    }
}
