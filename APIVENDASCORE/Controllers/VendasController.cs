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

namespace APIVENDASCORE.Controllers
{
    [ApiController]
    [Route("api/vendas/")]
    [Produces("application/json")]
    public class VendasController : ControllerBase
    {
        // LOGIN CPF

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] Usuario Login)
        {
            using (var ctx = new Contexto())
            {
                Criptografia cript = new Criptografia();
                var Cpf = Login.Cpf;

                string senhaCriptografada = ctx.Usuario
                    .Where(u => u.Cpf == Cpf)
                    .Select(u => u.Senha)
                    .FirstOrDefault();

                bool Autenticado = cript.ComparaMD5(Login.Senha, senhaCriptografada);

                if (senhaCriptografada == null || Autenticado == false)
                {
                    return BadRequest("Usuário ou senha inválidos");
                }

                var user = ctx.Usuario
                    .Where(u => u.Cpf == Cpf)
                    .Select(u => u.Tipo)
                    .FirstOrDefault();

                var roles = new List<string>();
                if (user == 0)
                {
                    roles.Add("Autenticado");
                }
                else if (user == 1)
                {
                    roles.Add("Administrador");
                    roles.Add("Default");
                }

                var token = TokenServices.GenerateToken(Login, roles.ToArray());
                Login.Senha = "";

                var result = new
                {
                    user = Login,
                    token = token,
                    role = roles
                };

                return result;
            }
        }

        // LOGIN VENDEDOR

        [HttpPost]
        [Route("LoginVendedor")]
        public async Task<ActionResult<dynamic>> AuthenticateVendedor([FromBody] Vendedor Login)
        {
            using (var ctx = new Contexto())
            {
                Criptografia cript = new Criptografia();
                var Cnpj = Login.Cnpj;

                string senhaCriptografada = ctx.Vendedor
                    .Where(u => u.Cnpj == Cnpj)
                    .Select(u => u.Senha)
                    .FirstOrDefault();

                bool Autenticado = cript.ComparaMD5(Login.Senha, senhaCriptografada);

                if (senhaCriptografada == null || Autenticado == false)
                {
                    return BadRequest("Usuário ou senha inválidos");
                }

                var user = ctx.Vendedor
                    .Where(u => u.Cnpj == Cnpj)
                    .Select(u => u.Tipo)
                    .FirstOrDefault();

                var roles = new List<string>();
                if (user == 0)
                {
                    roles.Add("Autenticado");
                }
                else if (user == 1)
                {
                    roles.Add("Administrador");
                    roles.Add("Default");
                }

                var token = TokenServices.GenerateTokenVendedor(Login, roles.ToArray());
                Login.Senha = "";

                var result = new
                {
                    user = Login,
                    token = token,
                    role = roles
                };

                return result;
            }
        }

        // LOGIN TRANSPORTADORA

        [HttpPost]
        [Route("LoginTransportadora")]
        public async Task<ActionResult<dynamic>> AuthenticateTransportadora([FromBody] Transportadora Login)
        {
            using (var ctx = new Contexto())
            {
                Criptografia cript = new Criptografia();
                var Cnpj = Login.Cnpj;

                string senhaCriptografada = ctx.Transportadora
                    .Where(u => u.Cnpj == Cnpj)
                    .Select(u => u.Senha)
                    .FirstOrDefault();

                bool Autenticado = cript.ComparaMD5(Login.Senha, senhaCriptografada);

                if (senhaCriptografada == null || Autenticado == false)
                {
                    return BadRequest("Usuário ou senha inválidos");
                }

                var user = ctx.Transportadora
                    .Where(u => u.Cnpj == Cnpj)
                    .Select(u => u.Tipo)
                    .FirstOrDefault();

                var roles = new List<string>();
                if (user == 0)
                {
                    roles.Add("Autenticado");
                }
                else if (user == 1)
                {
                    roles.Add("Administrador");
                    roles.Add("Default");
                }

                var token = TokenServices.GenerateTokenTransportadora(Login, roles.ToArray());
                Login.Senha = "";

                var result = new
                {
                    user = Login,
                    token = token,
                    role = roles
                };

                return result;
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
            catch (Exception err)
            {
                return BadRequest($"Erro ao cadastrar cliente: {err.Message}");
            }
        }

        //PESQUISAS

        //Retorna um pedido a partir do Codigo
        [HttpGet("BuscarPedido/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Busca um pedido especifico")]
        public Pedidos BuscarPedido(int id)
        {
            return VendasCRUD.BuscarPedido(id);
        }

        //RETORNA Lista de Historicos de um Pedido
        [HttpGet("BuscarHistorico/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Busca o historico de um pedido especifico")]
        public IEnumerable<HistPedido> BuscarHistorico(int id)
        {
            return VendasCRUD.ListarHistorico(id);
        }

        ////RETORNA data de um pedido entregue
        [HttpGet("DataEntrega/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Busca a data da entrega de um pedido")]
        public Nullable<DateTime> DataEntrega(int id)
        {
            return VendasCRUD.DataEntrega(id);
        }



        //Retorna todos os pedidos
        [HttpGet, Route("ListarPedidos")]
        [Authorize]
        public IEnumerable<Pedidos> ListarPedidos()
        {
            return VendasCRUD.ListarPedidos();
        }

        //Retorna todos os pedidos de um cliente especifico (busca por CPF)

        [HttpGet, Route("ListarPedidosCPF/{id}")]
        [Authorize]
        public IEnumerable<Pedidos> ListarPedidosCPF(string id)
        {
            return VendasCRUD.ListarPedidosCPF(id);
        }

        //Retorna todos os pedidos de um certo status (Entregues, Cancelados...)
        [HttpGet, Route("ListarPedidosStatus/{id}")]
        [Authorize]
        public IEnumerable<Pedidos> ListarPedidosStatus(int id)
        {
            return VendasCRUD.ListarPedidosStatus(id);
        }

        //Retorna o total de pedidos
        [HttpGet, Route("TotalPedidosFeitos")]
        [Authorize]
        public string TotalPedidos()
        {
            return VendasCRUD.TotalPedidosFeitos();
        }

        //OPERACOES

        //INCLUI UM NOVO PEDIDO

        [HttpPost, Route("NovoPedido")]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
