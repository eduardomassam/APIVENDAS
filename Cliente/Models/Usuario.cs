using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cliente.Models
{
    public class Usuario
    {
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public int Tipo { get; set; }
        public int Id { get; set; }
    }
}