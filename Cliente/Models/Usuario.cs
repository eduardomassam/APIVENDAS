using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cliente.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public int Tipo { get; set; }
        
    }
}