using System.Windows;
using System.Windows.Controls;

namespace CarDealerApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BuildTabs();
        }

        private void BuildTabs()
        {
            var vehiclesTab = new TabItem() { Header = "Veículos" };
            vehiclesTab.Content = new Views.VehiclesView();
            MainTabs.Items.Add(vehiclesTab);

            var filiaisTab = new TabItem() { Header = "Filiais" };
            filiaisTab.Content = new Views.FiliaisView();
            MainTabs.Items.Add(filiaisTab);

            var clientsTab = new TabItem() { Header = "Clientes" };
            clientsTab.Content = new Views.ClientsView();
            MainTabs.Items.Add(clientsTab);

            var salesTab = new TabItem() { Header = "Vendas" };
            salesTab.Content = new Views.SalesView();
            MainTabs.Items.Add(salesTab);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
