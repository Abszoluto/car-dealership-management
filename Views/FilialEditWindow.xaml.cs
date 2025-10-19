using CarDealerApp.ViewModels;
using System.Windows;

namespace CarDealerApp.Views
{
    public partial class FilialEditWindow : Window
    {
        public FilialEditWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is FilialEditViewModel vm)
            {
                if (string.IsNullOrWhiteSpace(vm.Filial.Nome) ||
                    string.IsNullOrWhiteSpace(vm.Filial.Cnpj) ||
                    string.IsNullOrWhiteSpace(vm.Filial.Estado) ||
                    string.IsNullOrWhiteSpace(vm.Filial.Cidade))
                {
                    MessageBox.Show("Por favor, preencha todos os campos (exceto Data de Criação).", "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            DialogResult = true;
            Close();
        }
    }
}
