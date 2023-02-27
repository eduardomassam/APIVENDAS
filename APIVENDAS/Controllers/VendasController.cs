using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


//Incluir namespaces necessarias
using APIVENDAS.Models;
using APIVENDAS.Dados;

namespace APIVENDAS.Controllers
{
    //prefixo da rota da API
    [RoutePrefix("api/vendas")]
    public class VendasController : ApiController
    {
        //Pesquisas

        //Retorna um pedido a partir do Codigo
        [HttpGet,Route("BuscarPedido/{id}")]
        public Pedidos BuscarPedido(int id)
        {
            return VendasCRUD.BuscarPedido(id);
        }

        //RETORNA Lista de Historicos de um Pedido
        [HttpGet,Route("BuscarHistorico/{id}")]
        public IEnumerable<HistPedido> BuscarHistorico (int id)
        {
            return VendasCRUD.ListarHistorico(id);
        }

        ////RETORNA data de um pedido entregue
        //[HttpGet, Route("DataEntrega/{id}")]
        //public HistPedido DataEntrega(int id)
        //{
        //    return VendasCRUD.DataEntrega(id);
        //}



        //Retorna todos os pedidos
        [HttpGet, Route("ListarPedidos")]
        public IEnumerable<Pedidos> ListarPedidos()
        {
            return VendasCRUD.ListarPedidos();
        }

        //Retorna todos os pedidos de um cliente especifico (busca por CPF)
        [HttpGet, Route("ListarPedidosCPF/{id}")]
        public IEnumerable<Pedidos> ListarPedidosCPF(string id)
        {
            return VendasCRUD.ListarPedidosCPF(id);
        }

        //Retorna todos os pedidos de um certo status (Entregues, Cancelados...)
        [HttpGet,Route("ListarPedidosStatus/{id}")]
        public IEnumerable<Pedidos> ListarPedidosStatus (string id)
        {
            return VendasCRUD.ListarPedidosStatus(id);
        }

        //Retorna o total de pedidos
        [HttpGet,Route("TotalPedidosFeitos")]
        public string TotalPedidos()
        {
            return VendasCRUD.TotalPedidosFeitos();
        }

        //OPERACOES
        //INCLUI UM NOVO PEDIDO

        [HttpPost,Route("NovoPedido")]
        public string IncluirPedido(Pedidos Novo)
        {
            try
            {
                VendasCRUD.NovoPedidoCliente(Novo);
                return "Ok";
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        //Altera o status de um pedido
        [HttpPost, Route("MudarStatusPedido")]
        public string MudarStatus(Status Mudou)
        {
            try
            {
                VendasCRUD.AlterarStatusPedido(Mudou);
                return "Ok";
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        //Faz a avaliação de um pedido por um cliente
        [HttpPost, Route("AvaliarPedido")]
        public string PedidoAvaliado(AvaliacaoPedido Info)
        {
            try
            {
                VendasCRUD.AvaliarPedido(Info);
                return "Ok";
            }
            catch(Exception err)
            {
                return err.Message;
            }
        }

        //Faz o cancelamento do Pedido pelo cliente
        [HttpPost, Route("CancelarPedido")]
        public string PedidoCancelado(AvaliacaoPedido Info)
        {
            try
            {
                VendasCRUD.CancelarPedido(Info);
                return "Ok";
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        //Devolver pedido durante 7 dias da chegada
        [HttpPost, Route("DevolverPedido")]
        public string PedidoDevolvido(AvaliacaoPedido Info)
        {
            try
            {
                
                VendasCRUD.DevolverPedido(Info);
                return "Ok";
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        [HttpPost, Route("DevolverPedidoTransportadora")]
        public string DevolverPedidoTransportadora(Status Info)
        {
            try
            {
                VendasCRUD.DevolverPedidoTransportadora(Info);
                return "Ok";
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        [HttpPost, Route("DevolverPedidoVendedor")]
        public string DevolverPedidoVendedor(Status Info)
        {
            try
            {
                VendasCRUD.DevolverPedidoTransportadora(Info);
                return "Ok";
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        [HttpPost, Route("DevolverPedidoVendedorAceite")]
        public string DevolverPedidoVendedorAceite(Status Info)
        {
            try
            {
                VendasCRUD.DevolverPedidoTransportadora(Info);
                return "Ok";
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

    }
}
