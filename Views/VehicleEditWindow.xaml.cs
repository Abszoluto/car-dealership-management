using CarDealerApp.ViewModels;
using System.Windows;
using System.Linq;

namespace CarDealerApp.Views
{
    public partial class VehicleEditWindow : Window
    {
        public VehicleEditWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is VeiculoEditViewModel vm)
            {
                if (vm.Veiculo.IdFilial == 0)
                {
                    MessageBox.Show("Por favor, selecione uma filial.", "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!vm.UpdateValorFromTexto())
                {
                    MessageBox.Show("O valor inserido não é um número válido.", "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            DialogResult = true;
            Close();
        }
    }
}
