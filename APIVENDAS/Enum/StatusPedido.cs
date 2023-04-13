using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIVENDAS.Enum
{
    public enum StatusPedido
    {
        FEITO = 1,
        ENTREGUE = 2,
        CANCELADO = 3,
        AVALIADO = 4,
        DEVOLVIDO_CLIENTE = 5,
        DEVOLVIDO_TRANSPORTADORA = 6,
        DEVOLVIDO_VENDEDOR = 7,
        DEVOLVIDO_COM_SUCESSO = 8,
        ENVIADO = 9,
        DEVOLUCAO_ACEITA = 10,
        EM_TRANSPORTE = 11
    }
}