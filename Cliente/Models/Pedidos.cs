using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cliente.Models
{
    public class Pedidos
    {
        public int Cod { get; set; }
        public string CPF { get; set; }
        public string NomeCliente { get; set; }
        public string Produto { get; set; }
        public Nullable<int> Quantidade { get; set; }
        public string Status { get; set; }

        //propSistemica

        public DateTime? DataOcorrencia { get; set; }
        public bool IsEnviar { get; set; }
    }
}