using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



// Namespaces para conexão com API
using System.Net.Http;
using System.Net.Http.Headers;

// Namespaces para uso dos dados da API
using System.Threading.Tasks;
using Newtonsoft.Json;

using System.Text;
using Cliente.Models;

namespace client.Controllers
{
    public class PedidosController : Controller
    {
        //Objeto de acesso a API
        HttpClient client;

        // constructor para conexão com a API
        public PedidosController()
        {
            // endereço da API e o tipo da resposta dela

            if(client==null)
            {
                client = new HttpClient();
                //porta da APIVENDAS abaixo
                client.BaseAddress = new Uri("https://localhost:44364/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        //Metodo para listar os pedidos do client (Via API)
        public async Task <ActionResult> Listar()
        {
            string API = "api/vendas/ListarPedidoscpf/" + Session["CPF"].ToString();
            var response = await client.GetAsync(API);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();

                //List<Pedidos> <======== Json
                var Lista = JsonConvert.DeserializeObject<Pedidos[]>(resultado).ToList();

                return View(Lista);
            }
            else return View();
        }

      [HttpGet]
      public ActionResult NovoPedido()
        {
            Pedidos Novo = new Pedidos();
            Novo.CPF = Session["CPF"].ToString();

            return View(Novo);
        }

        [HttpPost]
        public async Task<ActionResult> NovoPedido(Pedidos Novo)
        {
            if (Session["CPF"] is null)
            {
                return View("Expirada");
            }

            Novo.Cod = 0; //auto incremento
            Novo.Status = "FEITO"; //1º status = NOVO
            Novo.CPF = Session["CPF"].ToString();

            string json = JsonConvert.SerializeObject(Novo);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");
            var response = await client.PostAsync("api/vendas/novopedido", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Listar");
            else
                throw new Exception(response.ReasonPhrase);
        }

        [HttpGet]
        public ActionResult ConfirmarPedidoEntregue(string id)
        {
            Session["CodPedido"] = id;

            AvaliacaoPedido Novo = new AvaliacaoPedido();
            Novo.CodPedido = Convert.ToInt32(id);
            Novo.Avaliacao = "Escreva sua avaliação...";

            return View(Novo);
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmarPedidoEntregue(AvaliacaoPedido Mudou)
        {
            Mudou.CodPedido = Convert.ToInt32(Session["CodPedido"].ToString());

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
            Session["CodPedido"] = id;

            AvaliacaoPedido Novo = new AvaliacaoPedido();
            Novo.CodPedido = Convert.ToInt32(id);
            Novo.Avaliacao = "Descreva o motivo do cancelamento...";

            return View(Novo);
        }

        [HttpPost]
        public async Task<ActionResult> CancelarPedidoNaoEnviado(AvaliacaoPedido Mudou)
        {
            Mudou.CodPedido = Convert.ToInt32(Session["CodPedido"].ToString());

            string json = JsonConvert.SerializeObject(Mudou);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/CancelarPedido", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Listar");
            else
                throw new Exception(response.ReasonPhrase);
        }

        [HttpGet]
        public ActionResult DevolverPedido(string id)
        {
            Session["CodPedido"] = id;

            AvaliacaoPedido Novo = new AvaliacaoPedido();
            Novo.CodPedido = Convert.ToInt32(id);
            Novo.Avaliacao = "Escreva o motivo da devolução...";

            return View(Novo);
        }

        [HttpPost]
        public async Task<ActionResult> DevolverPedido(AvaliacaoPedido Mudou)
        {
            Mudou.CodPedido = Convert.ToInt32(Session["CodPedido"].ToString());

            string json = JsonConvert.SerializeObject(Mudou);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/DevolverPedido", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Listar");
            else
                throw new Exception(response.ReasonPhrase);
        }



    }
}
