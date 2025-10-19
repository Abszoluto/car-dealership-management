using CarDealerApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace CarDealerApp.ViewModels
{
    public class ClienteEditViewModel : ObservableObject
    {
        public Cliente Cliente { get; set; }
        public ObservableCollection<string> TiposPessoa { get; set; }

        public ClienteEditViewModel(Cliente cliente)
        {
            Cliente = cliente;
            TiposPessoa = new ObservableCollection<string>
            {
                "fisica",
                "juridica"
            };
        }
    }
}
