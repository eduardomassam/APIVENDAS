using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel.Design;
using Transportadora.Models;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Transportadora
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
       

        private static HttpClient client;
        private List<Pedidos> pedidos;


        //constructor para conexão com a API
        public HomePage()
        {
            InitializeComponent();
            BindingContext = this;
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

  
private async void Button_Clicked_Retirar(object sender, EventArgs e)
{
    try
    {    
        // Chamar o método para buscar os pedidos
        var response = await client.GetAsync("api/vendas/ListarPedidosStatus/9");
        response.EnsureSuccessStatusCode();
        pedidos = await response.Content.ReadAsAsync<List<Pedidos>>();
        //Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ListView.ItemsSource = pedidos;

        // Ir para a página de Pedidos a Retirar
        Navigation.PushAsync(new PedidosaRetirar());
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ocorreu um erro: {ex.Message}");
    }
    finally
    {


    }
}

        private void Button_Clicked_Aentregar(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PedidosaEntregar());
        }

        private void Button_Clicked_Entregues(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PedidosEntregues());
        }

    }
}
