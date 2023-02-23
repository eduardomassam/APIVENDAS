using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vendedor.Models
{
    public class Status
    {
        public int CodPedido { get; set; }
        public string NovoStatus { get; set; }
        public string Obs { get; set; }
    }
}