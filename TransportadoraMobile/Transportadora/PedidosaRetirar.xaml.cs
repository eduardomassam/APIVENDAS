using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Transportadora.Models;
using System.Net.Http.Headers;

namespace Transportadora
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PedidosaRetirar : ContentPage
    {
        HttpClient client;

        public PedidosaRetirar()
        {
            InitializeComponent();
            BindingContext = new Pedidos(); // onde ViewModel é a classe que contém a propriedade "pedidos"

            if (client == null)
            {
                client = new HttpClient();
                //porta da APIVENDAS abaixo
                client.BaseAddress = new Uri("https://localhost:7259/");
                client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        private async Task<List<Pedidos>> ListarPedidosStatus9()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/vendas/ListarPedidosStatus/9");
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
        public class Retirar
        {
            public string Id { get; set; }
            public string Pedido { get; set; }

        }
        public class NativeListView :
    ListView
        {
            public static readonly
    BindableProperty ItemsProperty = BindableProperty.Create
    ("Items", typeof(IEnumerable<PedidosaRetirar>), typeof(NativeListView), new List<PedidosaRetirar>());
            public IEnumerable<PedidosaRetirar> Items
            {
                get
                {
                    return (IEnumerable<PedidosaRetirar>)GetValue(ItemsProperty);
                }
                set
                {
                    SetValue(ItemsProperty, value);
                }
            }
            public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;
            public void NotifyItemSelected(object item)
            {
                if (ItemSelected != null)
                {
                    ItemSelected(this, new SelectedItemChangedEventArgs(item));
                }
            }
        }
    }
}