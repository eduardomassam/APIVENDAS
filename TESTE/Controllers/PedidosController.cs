
using Microsoft.AspNetCore.Mvc;

using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using TESTE.Models;
using Newtonsoft.Json;

namespace TESTE.Controllers
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

            cpf = "07788291978";

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
    }
}
