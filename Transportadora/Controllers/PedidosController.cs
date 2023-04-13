using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Transportadora.Models;

//Namespaces para conexão com API
using System.Net.Http;
using System.Net.Http.Headers;

//Namespaces para uso dos dados da API
using System.Threading.Tasks;
using Newtonsoft.Json;

using System.Text;

namespace Transportadora.Controllers
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
                client.BaseAddress = new Uri("https://localhost:44364/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

   

        public async Task<ActionResult> MudarStatusRetirado(string id)
        {
            Status Mudou = new Status();

            Mudou.CodPedido = Convert.ToInt32(id);
            Mudou.NovoStatus = (int)Enum.StatusPedido.EM_TRANSPORTE;
            Mudou.Obs = "[TRANSPORTADORA] Pedido a caminho da Entrega";

            string json = JsonConvert.SerializeObject(Mudou);
            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/MudarStatusPedido", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("RetirarPedidos");
            else
                throw new Exception(response.ReasonPhrase);

        }


        public async Task<ActionResult> MudarStatusEntregue(string id)
        {
            Status Mudou = new Status();

            Mudou.CodPedido = Convert.ToInt32(id);
            Mudou.NovoStatus = (int)Enum.StatusPedido.ENTREGUE;
            Mudou.Obs = "[TRANSPORTADORA] Pedido Entregue ao comprador";

            string json = JsonConvert.SerializeObject(Mudou);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/MudarStatusPedido", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("EntregarPedidos");
            else
                throw new Exception(response.ReasonPhrase);
        }

        public async Task<ActionResult> PedidosDevolvidos()
        {
            var response = await client.GetAsync("api/vendas/listarpedidosstatus/DEVOLVIDO");

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                var Lista = JsonConvert.DeserializeObject<Pedidos[]>(resultado).ToList();
                return View(Lista);
            }
            else return View();
        }


        public async Task<ActionResult> MudarStatusColetado(string id)
        {
            Status Mudou = new Status();

            Mudou.CodPedido = Convert.ToInt32(id);
            Mudou.NovoStatus = (int)Enum.StatusPedido.DEVOLVIDO_TRANSPORTADORA;
            Mudou.Obs = "[TRANSPORTADORA] Pedido Coletado com o cliente";

            string json = JsonConvert.SerializeObject(Mudou);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/MudarStatusPedido", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("EntregarPedidos");
            else
                throw new Exception(response.ReasonPhrase);
        }

       

        public async Task<ActionResult> MudarStatusDevolvivo(string id)
        {
            Status Mudou = new Status();

            Mudou.CodPedido = Convert.ToInt32(id);
            Mudou.NovoStatus = (int)Enum.StatusPedido.DEVOLVIDO_VENDEDOR;
            Mudou.Obs = "[TRANSPORTADORA] Pedido Devolvido ao Vendedor";

            string json = JsonConvert.SerializeObject(Mudou);

            HttpContent content = new StringContent(json, Encoding.Unicode, "application/json");

            var response = await client.PostAsync("api/vendas/MudarStatusPedido", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("EntregarPedidos");
            else
                throw new Exception(response.ReasonPhrase);
        }

        //// GET: Pedidos
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public async Task<ActionResult> RetirarPedidos()
        {
            var response = await client.GetAsync("api/vendas/listarpedidosstatus/9");

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                var Lista = JsonConvert.DeserializeObject<Pedidos[]>(resultado).ToList();
                return View(Lista);
            }
            else return View();
        }



        public async Task<ActionResult> EntregarPedidos()
        {
            var response = await client.GetAsync("api/vendas/listarpedidosstatus/11");

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                var Lista = JsonConvert.DeserializeObject<Pedidos[]>(resultado).ToList();
                return View(Lista);
            }
            else return View();
        }

        public async Task<ActionResult> PedidosEntregues()
        {
            var response = await client.GetAsync("api/vendas/listarpedidosstatus/2");

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                var Lista = JsonConvert.DeserializeObject<Pedidos[]>(resultado).ToList();
                return View(Lista);
            }
            else return View();
        }

        //PEDIDOS QUE O CLIENTE DEVOLVEU E ESTÁ AGUARDANDO A TRANSPORTADORA COLETAR PARA DEVOLVER AO VENDEDOR
        public async Task<ActionResult> ListaDevolvidosCliente()
        {
            var response = await client.GetAsync("api/vendas/listarpedidosstatus/5");

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                var Lista = JsonConvert.DeserializeObject<Pedidos[]>(resultado).ToList();
                return View(Lista);
            }
            else return View();
        }

        public async Task<ActionResult> ListaDevolvidosVendedor()
        {
            var response = await client.GetAsync("api/vendas/listarpedidosstatus/6");

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                var Lista = JsonConvert.DeserializeObject<Pedidos[]>(resultado).ToList();
                return View(Lista);
            }
            else return View();
        }

        public async Task<ActionResult> ListaDevolvidosEntregueVendedor()
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
    }
}