using APIVENDASCORE.Dados;
using APIVENDASCORE.Models;
using APIVENDASCORE.Repositorys.APIVENDASCORE.Repositories;
using APIVENDASCORE.Services;
using APIVENDASCORE.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.Owin;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Cliente.Models;
using Microsoft.Data.SqlClient;

namespace APIVENDASCORE.Controllers
{
    [ApiController]
    [Route("api/vendas/")]
    [Produces("application/json")]
    public class VendasController : ControllerBase
    {


        [HttpPost, Route("RedefinirSenha")]
        public EsqueceuSenhaViewModel RedefinirSenha(EsqueceuSenhaViewModel Novo)
        {
            try
            {
                var result = VendasCRUD.TrocarSenha(Novo);
                return result;
            }
            catch (Exception err)
            {
                throw new ArgumentException("Usuário ou senha inválidos");
            }
        }

        [HttpPost, Route("Login")]
        public LoginResult Logar(Usuario Novo)
        {
            try
            {
                var result = VendasCRUD.Login(Novo);
                return result;
            }
            catch (Exception err)
            {
                throw new ArgumentException("Usuário ou senha inválidos");
            }
        }

        [HttpPost, Route("LoginVendedor")]
        public LoginResult LogarVendedor(Vendedor Novo)
        {
            try
            {
                var result = VendasCRUD.LoginVendedor(Novo);
                return result;
            }
            catch (Exception err)
            {
                throw new ArgumentException("Usuário ou senha inválidos");
            }
        }

        [HttpPost, Route("LoginTransportadora")]
        public LoginResult LogarTransportadora(Transportadora Novo)
        {
            try
            {
                var result = VendasCRUD.LoginTransportadora(Novo);
                return result;
            }
            catch (Exception err)
            {
                throw new ArgumentException("Usuário ou senha inválidos");
            }
        }

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
            catch (Exception ex)
            {
                var sqlEx = ex.InnerException as SqlException;
                if (sqlEx != null && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
                {
                    // Ocorreu uma exceção de chave primária ou índice exclusivo duplicado
                    return BadRequest("Já existe um registro com esses dados.");
                }
                else
                {
                    // Outro tipo de exceção, tratar de acordo
                    return BadRequest($"Erro ao cadastrar cliente: {ex.Message}");
                }
            }
        }

        //PESQUISAS

        //Retorna um pedido a partir do Codigo
        [HttpGet("BuscarPedido/{id}")]
        //[Authorize]
        [SwaggerOperation(Summary = "Busca um pedido especifico")]
        public Pedidos BuscarPedido(int id)
        {
            return VendasCRUD.BuscarPedido(id);
        }

        //RETORNA Lista de Historicos de um Pedido
        [HttpGet("BuscarHistorico/{id}")]
       ////[Authorize]
        [SwaggerOperation(Summary = "Busca o historico de um pedido especifico")]
        public IEnumerable<HistPedido> BuscarHistorico(int id)
        {
            return VendasCRUD.ListarHistorico(id);
        }

        ////RETORNA data de um pedido entregue
        [HttpGet("DataEntrega/{id}")]
       //[Authorize]
        [SwaggerOperation(Summary = "Busca a data da entrega de um pedido")]
        public Nullable<DateTime> DataEntrega(int id)
        {
            return VendasCRUD.DataEntrega(id);
        }



        //Retorna todos os pedidos
        [HttpGet, Route("ListarPedidos")]
       //[Authorize]
        public IEnumerable<Pedidos> ListarPedidos()
        {
            return VendasCRUD.ListarPedidos();
        }

        //Retorna todos os pedidos de um cliente especifico (busca por CPF)

        [HttpGet, Route("ListarPedidosCPF/{id}")]
       //[Authorize]
        public IEnumerable<Pedidos> ListarPedidosCPF(string id)
        {
            return VendasCRUD.ListarPedidosCPF(id);
        }

        //Retorna todos os pedidos de um certo status (Entregues, Cancelados...)
        [HttpGet, Route("ListarPedidosStatus/{id}")]
       //[Authorize]
        public IEnumerable<Pedidos> ListarPedidosStatus(int id)
        {
            return VendasCRUD.ListarPedidosStatus(id);
        }

        //Retorna o total de pedidos
        [HttpGet, Route("TotalPedidosFeitos")]
       //[Authorize]
        public string TotalPedidos()
        {
            return VendasCRUD.TotalPedidosFeitos();
        }

        //OPERACOES

        //INCLUI UM NOVO PEDIDO

        [HttpPost, Route("NovoPedido")]
       //[Authorize]
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
       //[Authorize]
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
       //[Authorize]
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
       //[Authorize]
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
       //[Authorize]
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
       //[Authorize]
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
       //[Authorize]
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
       //[Authorize]
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
