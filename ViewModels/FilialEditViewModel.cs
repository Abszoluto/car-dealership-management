using CarDealerApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CarDealerApp.ViewModels
{
    public class FilialEditViewModel : ObservableObject
    {
        public Filial Filial { get; set; }

        public FilialEditViewModel(Filial filial)
        {
            Filial = filial;
        }
    }
}
