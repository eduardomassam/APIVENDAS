using APIVENDASCORE.Dados;
using APIVENDASCORE.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace APIVENDASCORE.Controllers
{
    [ApiController]
    [Route("api/vendas/")]
    [Produces("application/json")]
    public class VendasController : ControllerBase
    {   
        //CADASTRA UM NOVO CLIENTE

        [HttpPost("NovoCliente")]
        [SwaggerOperation(Summary = "Cadastra um novo cliente")]
        public IActionResult IncluirCliente([FromBody] Usuario Novo)
        {
            try
            {
                VendasCRUD.NovoCliente(Novo);
                return Ok("Cliente cadastrado com sucesso!");
            }
            catch (Exception err)
            {
                return BadRequest($"Erro ao cadastrar cliente: {err.Message}");
            }
        }

        //PESQUISAS

        //Retorna um pedido a partir do Codigo
        [HttpGet("BuscarPedido/{id}")]
        [SwaggerOperation(Summary = "Busca um pedido especifico")]
        public Pedidos BuscarPedido(int id)
        {
            return VendasCRUD.BuscarPedido(id);
        }

        //RETORNA Lista de Historicos de um Pedido
        [HttpGet("BuscarHistorico/{id}")]
        [SwaggerOperation(Summary = "Busca o historico de um pedido especifico")]
        public IEnumerable<HistPedido> BuscarHistorico(int id)
        {
            return VendasCRUD.ListarHistorico(id);
        }

        ////RETORNA data de um pedido entregue
        [HttpGet("DataEntrega/{id}")]
        [SwaggerOperation(Summary = "Busca a data da entrega de um pedido")]
        public Nullable<DateTime> DataEntrega(int id)
        {
            return VendasCRUD.DataEntrega(id);
        }



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
        [HttpGet, Route("ListarPedidosStatus/{id}")]
        public IEnumerable<Pedidos> ListarPedidosStatus(int id)
        {
            return VendasCRUD.ListarPedidosStatus(id);
        }

        //Retorna o total de pedidos
        [HttpGet, Route("TotalPedidosFeitos")]
        public string TotalPedidos()
        {
            return VendasCRUD.TotalPedidosFeitos();
        }

        //OPERACOES

        //INCLUI UM NOVO PEDIDO

        [HttpPost, Route("NovoPedido")]
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
        public string PedidoAvaliado(AvaliacaoPedidos Info)
        {
            try
            {
                VendasCRUD.AvaliarPedido(Info);
                return "Ok";
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        //Faz o cancelamento do Pedido pelo cliente
        [HttpPost, Route("CancelarPedido")]
        public string PedidoCancelado(AvaliacaoPedidos Info)
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
        public string PedidoDevolvido(AvaliacaoPedidos Info)
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
