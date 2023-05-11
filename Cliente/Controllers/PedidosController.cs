using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;



// Namespaces para conexão com API
using System.Net.Http;
using System.Net.Http.Headers;

// Namespaces para uso dos dados da API
using System.Threading.Tasks;
using Newtonsoft.Json;

using System.Text;
using Cliente.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;
using Cliente;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using Controller = System.Web.Mvc.Controller;
using System.Net;

namespace client.Controllers
{
    public class PedidosController : Controller
    {
        //Objeto de acesso a API
        HttpClient client;

        //constructor para conexão com a API
        public PedidosController()
        {
            // endereço da API e o tipo da resposta dela

            if (client == null)
            {
                client = new HttpClient();
                //porta da APIVENDAS abaixo
                client.BaseAddress = new Uri("https://localhost:7259/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        private T[] ObterItensFiltrados<T>(string chave) where T : struct
        {
            var valores = Request.QueryString[chave];
            if (valores == null)
            {
                return new T[0];
            }

            var itens = valores.Split(',');
            var result = new List<T>();
            foreach (var item in itens)
            {
                if (Enum.TryParse<T>(item, out var valor))
                {
                    result.Add(valor);
                }
            }

            return result.ToArray();
        }
        public System.Web.Mvc.ActionResult RedefinirSenha()
        {
            return View("RedefinirSenha");
        }

        [System.Web.Mvc.HttpPost]
        public async Task<System.Web.Mvc.ActionResult> AlterarSenha(EsqueceuSenhaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://localhost:7259/");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var usuario = new EsqueceuSenhaViewModel
                    {
                        Cpf = viewModel.Cpf,
                        SenhaAtual = viewModel.SenhaAtual,
                        SenhaNova = viewModel.SenhaNova
                    };
                    var json = JsonConvert.SerializeObject(usuario);
                    var content = new StringContent(json, Encoding.Unicode, "application/json");

                    var response = await httpClient.PostAsync("api/vendas/RedefinirSenha", content);
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.MensagemSucesso = "Senha alterada com sucesso!";
                        return View("RedefinirSenha", viewModel);
                    }
                    else
                    {
                        ViewBag.MensagemErro = "Erro ao redefinir a senha, favor confirmar os dados preenchidos.";
                        return View("RedefinirSenha", viewModel);
                    }
                }
            }
            else
            {
                return View("RedefinirSenha", viewModel);
            }
        }

        // Metodo para listar os pedidos do client (Via API)
        public async Task<System.Web.Mvc.ActionResult> Listar(string cpf, IEnumerable<string> status)
        {
            var token2 = Request.Cookies["token"]?.Value;

            if (token2 is null)
            {
                return View("Expirada");
            }

            using (var httpClient = new HttpClient())
            {
                var token = Request.Cookies["token"].Value;
                httpClient.BaseAddress = new Uri("https://localhost:7259/");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var cpfClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");
                var cpftoken = cpfClaim?.Value;

                if (cpftoken == null)
                {
                    ModelState.AddModelError(string.Empty, "Acesso não autorizado.");
                    return RedirectToAction("../Home/Index");
                }

                if (token == null)
                {
                    ModelState.AddModelError(string.Empty, "Acesso não autorizado.");
                    return RedirectToAction("../Home/Index");
                }

                string API = "api/vendas/ListarPedidosCPF/" + cpftoken;

                ViewBag.CPF = cpfClaim.Value;
                HttpResponseMessage response = await httpClient.GetAsync("api/vendas/ListarPedidosCPF/" + cpftoken);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadAsStringAsync();

                    //List<Pedidos> <======== Json
                    var lista = JsonConvert.DeserializeObject<List<Pedidos>>(resultado);


                    // Verifica se foi passado algum filtro de status
                    if (status != null && status.Any())
                    {
                        var listaStatus = status.Select(s => Convert.ToInt32(s)).ToList();
                        lista = lista.Where(p => listaStatus.Contains((int)p.Status)).ToList();
                    }

                    var lstEntregue = lista.Where(l => l.Status == 2);

                    foreach (var item in lstEntregue)
                    {
                        List<HistPedido> histPedido = (List<HistPedido>)await HistoricoPedido(item.Cod.ToString());


                        var pedidosHistentregues = histPedido.Where(t => t.Obs == "[TRANSPORTADORA] Pedido Entregue ao comprador").ToList();

                        DateTime? dataEntrega = pedidosHistentregues.Select(t => t.DataOcorrencia).FirstOrDefault();

                        if (dataEntrega.HasValue && dataEntrega.Value.AddDays(7) > DateTime.Now)
                            item.IsEnviar = true;

                        else
                            item.IsEnviar = false;
                    }

                    return View(lista);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Acesso não autorizado.");
                    return RedirectToAction("../Home/Index");
                }
            }
        }



        public async Task<List<HistPedido>> HistoricoPedido(string id)
        {

            var token2 = Request.Cookies["token"]?.Value;
            if (token2 is null)
            {
                return null;
            }
            ViewData["Pedido"] = id;
            var response = await client.GetAsync("api/vendas/BuscarHistorico/" + id);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();

                var Lista = JsonConvert.DeserializeObject<List<HistPedido>>(resultado).ToList();


                return Lista;
            }
            else
                return null;
        }

        public async Task<System.Web.Mvc.ActionResult> HistoricoPedidoCliente(string id)
        {

            var token = Request.Cookies["token"]?.Value;

            if (token is null)
            {
                return View("Expirada");
            }

            ViewData["Pedido"] = id;
            var response = await client.GetAsync("api/vendas/BuscarHistorico/" + id);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();

                var Lista = JsonConvert.DeserializeObject<HistPedido[]>(resultado).ToList();
                return View(Lista);
            }
            else
                return View();
        }

        [System.Web.Mvc.HttpGet]
        public System.Web.Mvc.ActionResult NovoPedido(string cpf)
        {
            Pedidos Novo = new Pedidos();
            //Novo.CPF = Session["CPF"].ToString();

            ViewBag.CPF = cpf;
            return View(Novo);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<System.Web.Mvc.ActionResult> NovoPedido(Pedidos Novo)
        {
            
            var token = Request.Cookies["token"]?.Value;

            if (token is null)
            {
                return View("Expirada");
            }

            Novo.Cod = 0; //auto incremento
            Novo.Status = 1; //1º status = NOVO


            using (var httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri("https://localhost:7259/");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var cpfClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");
                Novo.CPF = "";

                if (cpfClaim != null)
                {
                    Novo.CPF = cpfClaim.Value;
                }
                ViewBag.CPF = Novo.CPF;
                string json = JsonConvert.SerializeObject(Novo);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");
                //HttpResponseMessage response = await httpClient.PostAsync("api/vendas/ListarPedidosCPF/" + cpf);

                var response = await httpClient.PostAsync("api/vendas/NovoPedido", content);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Listar", "Pedidos", new { cpf = Novo.CPF });
                else
                {
                    ViewBag.MensagemErro = "Erro ao Solicitar Pedido, favor confirmar os dados preenchidos.";
                    return View("NovoPedido", Novo);
                }
                
               
            }
        }

        [System.Web.Mvc.HttpGet]
        public System.Web.Mvc.ActionResult ConfirmarPedidoEntregue(string id)
        {
            Session["CodPedido"] = id;

            var token = Request.Cookies["token"]?.Value;

            if (token is null)
            {
                return View("Expirada");
            }

            AvaliacaoPedido Novo = new AvaliacaoPedido();
            Novo.CodPedido = Convert.ToInt32(id);
            Novo.Avaliacao = "Escreva sua avaliação...";

            using (var httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri("https://localhost:7259/");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var cpfClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");

                ViewBag.CPF = cpfClaim.Value;
            }

            return View(Novo);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<System.Web.Mvc.ActionResult> ConfirmarPedidoEntregue(AvaliacaoPedido Mudou)
        {
            var token = Request.Cookies["token"]?.Value;

            if (token is null)
            {
                return View("Expirada");
            }

            using (var httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri("https://localhost:7259/");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var cpfClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");
                var CPF = cpfClaim.Value;

                Mudou.CodPedido = Convert.ToInt32(Session["CodPedido"].ToString());

                string json = JsonConvert.SerializeObject(Mudou);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await client.PostAsync("api/vendas/AvaliarPedido", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Listar", "Pedidos", new { CPF });
                else
                    throw new Exception(response.ReasonPhrase);
            }

        }

        [System.Web.Mvc.HttpGet]
        public System.Web.Mvc.ActionResult CancelarPedidoNaoEnviado(string id)
        {
            Session["CodPedido"] = id;

            var token = Request.Cookies["token"]?.Value;

            if (token is null)
            {
                return View("Expirada");
            }
            AvaliacaoPedido Novo = new AvaliacaoPedido();
            Novo.CodPedido = Convert.ToInt32(id);
            Novo.Avaliacao = "Descreva o motivo do cancelamento...";

            using (var httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri("https://localhost:7259/");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var cpfClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");



                ViewBag.CPF = cpfClaim.Value;
            }

            return View(Novo);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<System.Web.Mvc.ActionResult> CancelarPedidoNaoEnviado(AvaliacaoPedido Mudou)
        {
            var token = Request.Cookies["token"]?.Value;

            if (token is null)
            {
                return View("Expirada");
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7259/");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var cpfClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");

                var CPF = cpfClaim.Value;

                Mudou.CodPedido = Convert.ToInt32(Session["CodPedido"].ToString());

                string json = JsonConvert.SerializeObject(Mudou);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await httpClient.PostAsync("api/vendas/CancelarPedido", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Listar", "Pedidos", new { cpf = CPF });
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }

        [System.Web.Mvc.HttpGet]
        public System.Web.Mvc.ActionResult DevolverPedido(string id)
        {
            Session["CodPedido"] = id;

            var token = Request.Cookies["token"]?.Value;

            if (token is null)
            {
                return View("Expirada");
            }


            AvaliacaoPedido Novo = new AvaliacaoPedido();
            Novo.CodPedido = Convert.ToInt32(id);
            Novo.Avaliacao = "Descreva o motivo da devolução...";
            using (var httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri("https://localhost:7259/");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var cpfClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");

                ViewBag.CPF = cpfClaim.Value;
            }


            return View(Novo);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<System.Web.Mvc.ActionResult> DevolverPedido(AvaliacaoPedido Mudou)
        {
            var token = Request.Cookies["token"]?.Value;

            if (token is null)
            {
                return View("Expirada");
            }
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7259/");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var cpfClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");

                var CPF = cpfClaim.Value;
                Mudou.CodPedido = Convert.ToInt32(Session["CodPedido"].ToString());

                string json = JsonConvert.SerializeObject(Mudou);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await client.PostAsync("api/vendas/DevolverPedido", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Listar", "Pedidos", new { cpf = CPF });
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }




        //CADASTRAR NOVO CLIENTE
        [System.Web.Mvc.HttpGet]
        public System.Web.Mvc.ActionResult NovoCliente(string id)
        {
            Session["CodPedido"] = id;

            Usuario Novo = new Usuario();
            Novo.Cpf = "";
            Novo.Senha = "";
            return View(Novo);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<System.Web.Mvc.ActionResult> NovoCliente(Usuario Novo)
        {
            var validator = new UsuarioValidator();
            var validationResult = validator.Validate(Novo);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(Novo);
            }

            string json = JsonConvert.SerializeObject(Novo);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/NovoCliente", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.MensagemSucesso = "Cliente cadastrado com sucesso!";
                return View("NovoCliente", Novo);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string mensagemErro = await response.Content.ReadAsStringAsync();
                if (mensagemErro.Contains("Já existe um registro"))
                {
                    ViewBag.MensagemErro = "Já existe um cliente cadastrado com este CPF.";
                }
                else
                {
                    ViewBag.MensagemErro = mensagemErro;
                }
                return View("NovoCliente", Novo);
            }
            else
            {
                ViewBag.MensagemErro = "Erro ao cadastrar cliente.";
                return View("NovoCliente", Novo);
            }
        }

    }
}
