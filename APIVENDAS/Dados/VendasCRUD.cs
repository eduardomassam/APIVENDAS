using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using APIVENDAS.Models;
using System.Data.Entity;

namespace APIVENDAS.Dados
{
    public class VendasCRUD
    {
        //PESQUISAS
        
        //RETORNA um pedido em especifico a partir de seu codigo
        public static Pedidos BuscarPedido(int Codigo)
        {
            using (var ctx = new VendasEntities())
            {
                return ctx.Pedidos.FirstOrDefault(p => p.Cod.Equals(Codigo));
            }
        }


        //RETORNA todo o historico de um pedido a partir de seu codigo
        public static IEnumerable<HistPedido> ListarHistorico (int Codigo)
        {
            using (var ctx = new VendasEntities())
            {
                var Pesquisa = (from A in ctx.HistPedido where A.CodPed == Codigo select A).ToList();
                return Pesquisa;
            }
        }

        ////RETORNA Data Entrega
        //public static HistPedido DataEntrega(int Codigo)
        //{
        //    using (var ctx = new VendasEntities())
        //    {                
        //        var Pesquisa = (from A in ctx.HistPedido where A.CodPed == Codigo select A).ToList();
        //        var x = Pesquisa.Last();
        //        var y = x.DataOcorrencia;
        //        return Pesquisa;
        //    }
        //}

      





        //RETORNA todos os pedidos
        public static IEnumerable<Pedidos> ListarPedidos()
        {
            using (var ctx=new VendasEntities())
            {
                return ctx.Pedidos.ToList();
            }
        }

        //RETORNA todos os pedidos de um determinado CPF
        public static IEnumerable<Pedidos> ListarPedidosCPF (string Codigo)
        {
            using (var ctx = new VendasEntities())
            {
                var Pesquisa = (from A in ctx.Pedidos where A.CPF == Codigo select A).ToList();
                return Pesquisa;
            }
        }

        //RETORNA todos os pedidos de um determinado STATUS
        public static IEnumerable<Pedidos> ListarPedidosStatus(string status)
        {
            using (var ctx = new VendasEntities())
            {
                var Pesquisa = (from A in ctx.Pedidos where A.Status == status select A).ToList();
                return Pesquisa;
            }
        }

        //RETORNA o total de novos Pedidos
        public static string TotalPedidosFeitos()
        {
            using (var ctx = new VendasEntities())
            {
                var Pesquisa = (from A in ctx.Pedidos where A.Status == "FEITO" select A).Count().ToString();
                return Pesquisa;
            }
        }

        //OPERAÇÕES

        //INCLUI um Novo Historico (Pedido 1=>N Historicos)
        public static void NovoHistorico (HistPedido Novo)
        {
            using (var ctx = new VendasEntities())
            {
                ctx.HistPedido.Add(Novo);
                ctx.SaveChanges();
            }
        }

        //INCLUI um historico a partir da mudança de status ou outro evento
        public static void IncluirHistorico(int CodPedido,string Obs)
        {
            HistPedido NovoHist = new HistPedido();
            NovoHist.CodPed = CodPedido;
            NovoHist.NroSeq = 0; //CAMPO INCREMENTAL
            NovoHist.DataOcorrencia = DateTime.Now;
            NovoHist.Obs = Obs;

            NovoHistorico(NovoHist);
        }

        //INCLUI um NOVO pedido. Imediatamente inclui o 1º Historico
        public static void NovoPedidoCliente(Pedidos Novo)
        {
            using (var ctx = new VendasEntities())
            {
                ctx.Pedidos.Add(Novo);
                ctx.SaveChanges();
            }
            IncluirHistorico(Novo.Cod, "[CLIENTE] Pedido Incluido");
        }

        //Altera um pedido
        public static void AlterarPedido(Pedidos Alterado)
        {
            using (var ctx = new VendasEntities())
            {
                ctx.Entry<Pedidos>(Alterado).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        //AUXILIARES
        public static string UltimoCodigoIncluido()
        {
            using (var ctx = new VendasEntities())
            {
                var Pesquisa = ctx.Pedidos.Max(a => a.Cod).ToString();
                return Pesquisa;
            }
        }

        //ALTERA O STATUS DE UM PEDIDO a partir da classe status
        //IMEDIATAMENTE após a mudança, incluir um historico
        public static void AlterarStatusPedido(Status Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = Info.NovoStatus;
            AlterarPedido(Alt);

            IncluirHistorico(Alt.Cod, Info.Obs);
        }

        //FAZ a avaliacao de um pedido
        public static void AvaliarPedido(AvaliacaoPedido Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = "AVALIADO";
            AlterarPedido(Alt);

            Info.Avaliacao = "[CLIENTE] Avaliação: " + Info.Avaliacao;
            IncluirHistorico(Alt.Cod, Info.Avaliacao);

        }
        
        //Cliente cancelar pedido não enviado ainda
        public static void CancelarPedido(AvaliacaoPedido Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = "CANCELADO";
            AlterarPedido(Alt);

            Info.Avaliacao = "[CLIENTE] Motivo do Cancelamento: " + Info.Avaliacao;

            IncluirHistorico(Alt.Cod, Info.Avaliacao);
        }

        //Devolver pedido dentro de 7 dias da entrega
        public static void DevolverPedido(AvaliacaoPedido Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = "DEVOLVIDO_CLIENTE";
            AlterarPedido(Alt);

            Info.Avaliacao = "[CLIENTE] Motivo da Devolução: " + Info.Avaliacao;

            IncluirHistorico(Alt.Cod, Info.Avaliacao);
        }

        //Transportadora coletar o pedido com o cliente para devolução ao vendedor
        public static void DevolverPedidoTransportadora(Status Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = "DEVOLVIDO_TRANSPORTADORA";
            AlterarPedido(Alt);

            IncluirHistorico(Alt.Cod, Info.Obs);
        }

        //Transportadora entregar o pedido devolvivo ao vendedor
        public static void DevolverPedidoVendedor(Status Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = "DEVOLVIDO_VENDEDOR";
            AlterarPedido(Alt);

            IncluirHistorico(Alt.Cod, Info.Obs);
        }

        //Vendedor aceitar a devolução
        public static void DevolverPedidoVendedorAceite(Status Info)
        {
            Pedidos Alt = BuscarPedido(Info.CodPedido);
            Alt.Status = "DEVOLVIDO_COM_SUCESSO";
            AlterarPedido(Alt);

            IncluirHistorico(Alt.Cod, Info.Obs);
        }

    }
}