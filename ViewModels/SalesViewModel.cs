using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Dapper;
using CarDealerApp.Models;
using CarDealerApp.Views;
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CarDealerApp.ViewModels
{
    public class SalesViewModel : ObservableObject
    {
        public ObservableCollection<PedidoVendaView> Vendas { get; set; } = new();
        public ICommand CreateSaleCommand { get; }
        public ICommand RefreshCommand { get; }

        public SalesViewModel()
        {
            CreateSaleCommand = new RelayCommand(CreateSale);
            RefreshCommand = new RelayCommand(Load);
            Load();
        }

        public void Load()
        {
            Vendas.Clear();
            using var db = new Data.Database(MainViewModel.DbPath);
            var rows = db.Connection.Query<dynamic>(@"SELECT p.*, c.nome_razao as ClienteNome, v.marca || ' ' || v.modelo || ' (' || v.ano_modelo || ')' as VeiculoInfo
                FROM pedido_venda p
                LEFT JOIN cliente c ON p.id_cliente=c.id_cliente
                LEFT JOIN veiculo v ON p.id_venda = v.id_veiculo;").ToList();
            foreach (var r in rows)
            {
                Vendas.Add(new PedidoVendaView
                {
                    IdVenda = r.id_venda,
                    ClienteNome = r.ClienteNome,
                    VeiculoInfo = r.VeiculoInfo,
                    Status = r.status,
                    DataVenda = r.data_venda
                });
            }
        }

        private void CreateSale()
        {
            using var db = new Data.Database(MainViewModel.DbPath);
            var clientes = db.Connection.Query<Cliente>("SELECT * FROM cliente;").ToList();
            var veiculos = db.Connection.Query<Veiculo>("SELECT * FROM veiculo WHERE status IS NULL OR status!='Vendido';").ToList();
            var win = new SaleCreateWindow(clientes, veiculos);
            if (win.ShowDialog() == true)
            {
                var dto = new {
                    IdFilial = 1,
                    IdCliente = win.SelectedClienteId,
                    IdVendedor = 1,
                    Status = "concluida",
                    DataVenda = win.SelectedDate.ToString("yyyy-MM-dd"),
                    DataCriacao = DateTime.Now.ToString("yyyy-MM-dd")
                };
                var id = db.Connection.ExecuteScalar<long>(@"INSERT INTO pedido_venda (id_filial,id_cliente,id_vendedor,status,data_venda,data_criacao) VALUES (@IdFilial,@IdCliente,@IdVendedor,@Status,@DataVenda,@DataCriacao); SELECT last_insert_rowid();", dto);
                db.Connection.Execute("UPDATE veiculo SET status='Vendido' WHERE id_veiculo=@IdVeiculo;", new { IdVeiculo = win.SelectedVeiculoId });
                Load();
            }
        }
    }

    public class PedidoVendaView
    {
        public int IdVenda { get; set; }
        public string ClienteNome { get; set; }
        public string VeiculoInfo { get; set; }
        public string Status { get; set; }
        public string DataVenda { get; set; }
    }
}
