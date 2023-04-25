using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//Namespaces para conexão com API
using System.Net.Http;
using System.Net.Http.Headers;

//Namespaces para uso dos dados da API
using System.Threading.Tasks;
using Newtonsoft.Json;

using System.Text;
using Vendedor.Models;

namespace Vendedor.Controllers
{
    public class PedidosController : Controller
    {

        //Objeto de acesso a API
        HttpClient client;

        // constructor para conexão com a API
        public PedidosController()
        {
            // endereço da API e o tipo da resposta dela

            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7259/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        // GET: Pedidos
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public async Task<ActionResult> NovosPedidos()
        {
            var response = await client.GetAsync("api/vendas/listarpedidosstatus/1");
            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                var Lista = JsonConvert.DeserializeObject<Pedidos[]>(resultado).ToList();
                return View(Lista);
            }
            else return View();
        }

        public async Task<ActionResult> ListaPedidos()
        {
            var response = await client.GetAsync("api/vendas/listarpedidos");

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();

                var Lista = JsonConvert.DeserializeObject<Pedidos[]>(resultado).ToList();
                return View(Lista);
            }
            else return View();
        }


        public async Task<ActionResult> MudarStatus(string id)
        {
            Status Mudou = new Status();
            Mudou.CodPedido = Convert.ToInt32(id);
            Mudou.NovoStatus = (int)Enum.StatusPedido.ENVIADO;
            Mudou.Obs = "[VENDEDOR] Pedido disponível para transporte";

            string json = JsonConvert.SerializeObject(Mudou);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/MudarStatusPedido", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("NovosPedidos");
            else
                throw new Exception(response.ReasonPhrase);
        }

        public async Task<ActionResult> MudarStatusDevolvido(string id)
        {
            Status Mudou = new Status();
            Mudou.CodPedido = Convert.ToInt32(id);
            Mudou.NovoStatus = (int)Enum.StatusPedido.DEVOLUCAO_ACEITA;
            Mudou.Obs = "[VENDEDOR] Pedido devolvido com sucesso";

            string json = JsonConvert.SerializeObject(Mudou);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/MudarStatusPedido", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("PedidosDevolvidos");
            else
                throw new Exception(response.ReasonPhrase);
        }

        public async Task<ActionResult> HistoricoPedido (string id)
        {
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



        [HttpGet]
        public ActionResult VendedorCancelarPedidoNaoEnviado(string id)
        {
            Session["CodPedido"] = id;

            VendedorAvaliacaoPedido Novo = new VendedorAvaliacaoPedido();
            Novo.CodPedido = Convert.ToInt32(id);
            Novo.Avaliacao = "Descreva o motivo do cancelamento...";

            return View(Novo);
        }

        [HttpPost]
        public async Task<ActionResult> VendedorCancelarPedidoNaoEnviado(VendedorAvaliacaoPedido Mudou)
        {
            Mudou.CodPedido = Convert.ToInt32(Session["CodPedido"].ToString());

            string json = JsonConvert.SerializeObject(Mudou);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/CancelarPedido", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("ListaPedidos");
            else
                throw new Exception(response.ReasonPhrase);
        }

        [HttpPost]
        public async Task<ActionResult> VendedorAceitarDevolucao(VendedorAvaliacaoPedido Mudou)
        {
            Mudou.CodPedido = Convert.ToInt32(Session["CodPedido"].ToString());

            string json = JsonConvert.SerializeObject(Mudou);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/DevolverPedidoVendedorAceite", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("ListaPedidos");
            else
                throw new Exception(response.ReasonPhrase);
        }

        public async Task<ActionResult> PedidosDevolvidos()
        {
            var response = await client.GetAsync("api/vendas/listarpedidosstatus/7");

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                var Lista = JsonConvert.DeserializeObject<Pedidos[]>(resultado).ToList();
                return View(Lista);
            }
            else return View();
        }

        [HttpPost]
        public async Task<ActionResult> VedendorAceitarDevolucao(VendedorAvaliacaoPedido Mudou)
        {
            Mudou.CodPedido = Convert.ToInt32(Session["CodPedido"].ToString());

            string json = JsonConvert.SerializeObject(Mudou);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/DevolverPedidoVendedorAceite", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("ListaPedidos");
            else
                throw new Exception(response.ReasonPhrase);
        }
    }
}