﻿using System.ComponentModel.DataAnnotations;

namespace APIVENDASCORE.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public int Tipo { get; set; }
    }
}