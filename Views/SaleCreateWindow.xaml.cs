using System;
using System.Collections.Generic;
using System.Windows;

namespace CarDealerApp.Views
{
    public partial class SaleCreateWindow : Window
    {
        public int SelectedClienteId { get; private set; }
        public int SelectedVeiculoId { get; private set; }
        public DateTime SelectedDate { get; private set; }

        public SaleCreateWindow(IEnumerable<object> clientes, IEnumerable<object> veiculos)
        {
            InitializeComponent();
            ClienteCombo.ItemsSource = clientes;
            VeiculoCombo.ItemsSource = veiculos;
            DataVenda.SelectedDate = DateTime.Now;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (ClienteCombo.SelectedValue == null || VeiculoCombo.SelectedValue == null)
            {
                MessageBox.Show("Selecione cliente e veículo.");
                return;
            }
            SelectedClienteId = Convert.ToInt32(ClienteCombo.SelectedValue);
            SelectedVeiculoId = Convert.ToInt32(VeiculoCombo.SelectedValue);
            SelectedDate = DataVenda.SelectedDate ?? DateTime.Now;
            DialogResult = true;
            Close();
        }
    }
}
