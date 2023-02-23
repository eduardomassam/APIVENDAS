using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIVENDAS.Models
{
    public class Status
    {
        public int CodPedido { get; set; }
        public string NovoStatus { get; set; }
        public string Obs { get; set; }
        public DateTime Data { get; set; }
    }
}