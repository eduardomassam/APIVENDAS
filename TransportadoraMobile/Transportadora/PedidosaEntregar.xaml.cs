using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Transportadora.Models;

namespace Transportadora
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PedidosaEntregar : ContentPage
    {
        private object client;

        public PedidosaEntregar()
        {
            InitializeComponent();
        }

        public class NativeListView :
ListView
        {
            public static readonly
BindableProperty ItemsProperty = BindableProperty.Create
("Items", typeof(IEnumerable<PedidosaEntregar>), typeof(NativeListView), new List<PedidosaEntregar>());
            public IEnumerable<PedidosaEntregar> Items
            {
                get
                {
                    return (IEnumerable<PedidosaEntregar>)GetValue(ItemsProperty);
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