//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;
//using Transportadora.Models;

//namespace Transportadora.ViewModels
//{
//    public class PedidosARetirarViewModel : INotifyPropertyChanged
//    {
//        public ObservableCollection<Pedidos> Items { get; set; }

//        public PedidosARetirarViewModel()
//        {
//            Items = new ObservableCollection<Pedidos>();
//            // Aqui você pode preencher a lista com os pedidos a serem exibidos
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}