using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cliente.Models
{
    public class Pedidos
    {
        public int Cod { get; set; }
        public string CPF { get; set; }
        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        public string NomeCliente { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        public string Produto { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior do que zero.")]
        public Nullable<int> Quantidade { get; set; }
        public int Status { get; set; }

        //propSistemica

        public DateTime? DataOcorrencia { get; set; }
        public bool IsEnviar { get; set; }
    }
}