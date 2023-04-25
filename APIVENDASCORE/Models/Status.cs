using System;
using System.ComponentModel.DataAnnotations;

namespace APIVENDASCORE.Models
{
    public class Status
    {
        [Key]
        public int CodPedido { get; set; }
        public int NovoStatus { get; set; }
        public string Obs { get; set; }
        public DateTime Data { get; set; }
    }
}
