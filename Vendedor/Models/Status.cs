using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vendedor.Models
{
    public class Status
    {
        public int CodPedido { get; set; }
        public int NovoStatus { get; set; }
        public string Obs { get; set; }
    }
}