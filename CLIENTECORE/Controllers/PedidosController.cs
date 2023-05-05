﻿using CLIENTECORE.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CLIENTECORE.Controllers
{
    public class PedidosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

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


        // Metodo para listar os pedidos do client (Via API)
        public async Task<ActionResult> Listar(string cpf)
        {
            if (cpf == null)
            {
                return RedirectToAction("../Home/Index");
            }

            // Obtém o token armazenado no cookie
            var token = Request.Cookies["token"];

            if (token == null)
            {

            }

            string API = "api/vendas/ListarPedidosCPF/" + cpf;

            using (var httpClient = new HttpClient())
            {

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
                ViewBag.CPF = cpfClaim.Value;
                HttpResponseMessage response = await httpClient.GetAsync("api/vendas/ListarPedidosCPF/" + cpf);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadAsStringAsync();

                    //List<Pedidos> <======== Json
                    var lista = JsonConvert.DeserializeObject<List<Pedidos>>(resultado);

                    var lstEntregue = lista.Where(l => l.Status == 2);

                    foreach (var item in lstEntregue)
                    {
                        List<HistPedido> histPedido = await HistoricoPedido(item.Cod.ToString());

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


        [HttpGet]
        public ActionResult NovoPedido(string cpf)
        {
            Pedidos Novo = new Pedidos();
            //Novo.CPF = Session["CPF"].ToString();

            ViewBag.CPF = cpf;
            return View(Novo);
        }

        [HttpPost]
        public async Task<ActionResult> NovoPedido(Pedidos Novo)
        {
            var token = Request.Cookies["token"];

            if (token is null)
            {
                return View("Expirada");
            }

            Novo.Cod = 0; //auto incremento
            Novo.Status = 1; //1º status = NOVO
                             //Novo.CPF = Session["CPF"].ToString();





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
                    throw new Exception(response.ReasonPhrase);
            }
        }

        [HttpGet]
        public ActionResult ConfirmarPedidoEntregue(string id)
        {
            HttpContext.Session.SetString("CodPedido", id);

            AvaliacaoPedido Novo = new AvaliacaoPedido();
            Novo.CodPedido = Convert.ToInt32(id);
            Novo.Avaliacao = "Escreva sua avaliação...";

            return View(Novo);
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmarPedidoEntregue(AvaliacaoPedido Mudou)
        {
            Mudou.CodPedido = Convert.ToInt32(HttpContext.Session.GetString("CodPedido"));

            string json = JsonConvert.SerializeObject(Mudou);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/AvaliarPedido", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Listar");
            else
                throw new Exception(response.ReasonPhrase);
        }

        [HttpGet]
        public ActionResult CancelarPedidoNaoEnviado(string id)
        {
            HttpContext.Session.SetString("CodPedido", id);

            AvaliacaoPedido Novo = new AvaliacaoPedido();
            Novo.CodPedido = Convert.ToInt32(id);
            Novo.Avaliacao = "Descreva o motivo do cancelamento...";

            return View(Novo);
        }

        [HttpPost]
        public async Task<ActionResult> CancelarPedidoNaoEnviado(AvaliacaoPedido Mudou)
        {
            var token = Request.Cookies["token"];

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

                Mudou.CodPedido = Convert.ToInt32(HttpContext.Session.GetString("CodPedido"));

                string json = JsonConvert.SerializeObject(Mudou);

                HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

                var response = await httpClient.PostAsync("api/vendas/CancelarPedido", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Listar", "Pedidos", new { cpf = CPF });
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }

        [HttpGet]
        public ActionResult DevolverPedido(string id)
        {
            HttpContext.Session.SetString("CodPedido", id);

            AvaliacaoPedido Novo = new AvaliacaoPedido();
            Novo.CodPedido = Convert.ToInt32(id);
            Novo.Avaliacao = "Descreva o motivo da devolução...";
            return View(Novo);
        }

        [HttpPost]
        public async Task<ActionResult> DevolverPedido(AvaliacaoPedido Mudou)
        {
            Mudou.CodPedido = Convert.ToInt32(HttpContext.Session.GetString("CodPedido"));

            string json = JsonConvert.SerializeObject(Mudou);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/DevolverPedido", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Listar");
            else
                throw new Exception(response.ReasonPhrase);
        }


        public async Task<List<HistPedido>> HistoricoPedido(string id)
        {
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


        //CADASTRAR NOVO CLIENTE
        [HttpGet]
        public ActionResult NovoCliente(string id)
        {
            HttpContext.Session.SetString("CodPedido", id);

            Usuario Novo = new Usuario();
            Novo.Cpf = "";
            Novo.Senha = "";
            return View(Novo);
        }

        [HttpPost]
        public async Task<ActionResult> NovoCliente(Usuario Novo)
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
                //return RedirectToAction("../Home/Index");
                return View("NovoCliente", Novo);
            }
            else
            {
                ViewBag.MensagemErro = "Erro ao cadastrar cliente";
                return View("NovoCliente", Novo);
                //throw new Exception(response.ReasonPhrase);
            }
        }


    }
}