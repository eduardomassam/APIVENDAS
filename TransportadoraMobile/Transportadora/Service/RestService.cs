using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Transportadora.Models;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace Transportadora.Service
{
    public class RestService
    {
        HttpClient client;
        string urlServer = "https://localhost:7259/";

        public RestService()
        {
            if (client == null)
            {
                client = new HttpClient();
                //porta da APIVENDAS abaixo
                client.BaseAddress = new Uri("https://localhost:7259/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        //public async Task<List<Pedidos>> GetResultAsync()
        //{
        //return null;
        // }

        private async Task<List<Pedidos>> ListarPedidosStatus6()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/vendas/listarpedidosstatus/6");
                response.EnsureSuccessStatusCode();
                List<Pedidos> pedidos = await response.Content.ReadAsAsync<List<Pedidos>>();
                return pedidos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                return null;
            }
        }
    }
}
