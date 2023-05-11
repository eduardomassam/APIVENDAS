using System;
using System.Collections.Generic;
using System.Text;

namespace Transportadora.Models
{
    internal class Pedidos
    {
        public int Cod { get; set; }
        public string CPF { get; set; }
        public string NomeCliente { get; set; }
        public string Produto { get; set; }
        public int Quantidade { get; set; }
        public string Status { get; set; }
    }
}
