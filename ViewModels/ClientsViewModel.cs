using CarDealerApp.Models;
using CarDealerApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dapper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CarDealerApp.ViewModels
{
    public class ClientsViewModel : ObservableObject
    {
        public ObservableCollection<Cliente> Clientes { get; set; }

        private Cliente _selected;
        public Cliente Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        public ClientsViewModel()
        {
            Clientes = new ObservableCollection<Cliente>();
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, CanEditOrDelete);
            DeleteCommand = new RelayCommand(Delete, CanEditOrDelete);
            RefreshCommand = new RelayCommand(Load);
            Load();
        }

        public void Load()
        {
            Clientes.Clear();
            try
            {
                using var db = new Data.Database(MainViewModel.DbPath);
                var query = @"
                    SELECT id_cliente AS IdCliente, tipo_pessoa AS TipoPessoa, nome_razao AS NomeRazao,
                           documento AS Documento, email AS Email, telefone AS Telefone, data_cadastro AS DataCadastro
                    FROM cliente;";
                var rows = db.Connection.Query<Cliente>(query).ToList();
                foreach (var r in rows) Clientes.Add(r);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar clientes: {ex.Message}", "Erro de Banco de Dados", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string? ValidateClient(Cliente cliente, bool isEditing = false)
        {
            if (string.IsNullOrWhiteSpace(cliente.TipoPessoa))
                return "O Tipo de Pessoa é obrigatório.";

            if (string.IsNullOrWhiteSpace(cliente.NomeRazao))
                return "O campo Nome / Razão Social é obrigatório.";

            if (string.IsNullOrWhiteSpace(cliente.Documento))
                return "O campo Documento é obrigatório.";

            var docLimpo = Regex.Replace(cliente.Documento, @"[^\d]", "");
            if (cliente.TipoPessoa == "fisica" && docLimpo.Length != 11)
                return "O CPF deve conter 11 dígitos.";
            if (cliente.TipoPessoa == "juridica" && docLimpo.Length != 14)
                return "O CNPJ deve conter 14 dígitos.";

            if (!string.IsNullOrWhiteSpace(cliente.Telefone))
            {
                var phoneDigits = new string(cliente.Telefone.Where(char.IsDigit).ToArray());
                if (phoneDigits.Length != 11)
                {
                    return "O Telefone deve conter 11 dígitos (DDD + número de telemóvel).";
                }
            }

            if (!string.IsNullOrWhiteSpace(cliente.Email) && !Regex.IsMatch(cliente.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "O formato do email é inválido.";

            try
            {
                using var db = new Data.Database(MainViewModel.DbPath);
                string query = isEditing
                    ? "SELECT COUNT(1) FROM cliente WHERE documento = @Documento AND id_cliente != @IdCliente"
                    : "SELECT COUNT(1) FROM cliente WHERE documento = @Documento";

                var existe = db.Connection.ExecuteScalar<bool>(query, cliente);
                if (existe)
                    return "Este documento já está cadastrado.";
            }
            catch (Exception ex)
            {
                return $"Erro ao validar documento no banco de dados: {ex.Message}";
            }

            return null;
        }

        private void Add()
        {
            var newClient = new Cliente();
            var vm = new ClienteEditViewModel(newClient);

            bool isValid = false;
            while (!isValid)
            {
                var win = new ClientEditWindow() { DataContext = vm };
                if (win.ShowDialog() != true)
                {
                    return;
                }

                string? error = ValidateClient(newClient);
                if (error != null)
                {
                    MessageBox.Show(error, "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }

                isValid = true;
                try
                {
                    newClient.DataCadastro = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    using var db = new Data.Database(MainViewModel.DbPath);
                    var sql = @"
                        INSERT INTO cliente (tipo_pessoa, nome_razao, documento, email, telefone, data_cadastro)
                        VALUES (@TipoPessoa, @NomeRazao, @Documento, @Email, @Telefone, @DataCadastro);
                        SELECT last_insert_rowid();";
                    var id = db.Connection.ExecuteScalar<long>(sql, newClient);
                    newClient.IdCliente = (int)id;
                    Clientes.Add(newClient);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao salvar cliente: {ex.Message}", "Erro de Banco de Dados", MessageBoxButton.OK, MessageBoxImage.Error);
                    isValid = false;
                }
            }
        }

        private void Edit()
        {
            var copy = new Cliente
            {
                IdCliente = Selected.IdCliente,
                TipoPessoa = Selected.TipoPessoa,
                NomeRazao = Selected.NomeRazao,
                Documento = Selected.Documento,
                Email = Selected.Email,
                Telefone = Selected.Telefone,
                DataCadastro = Selected.DataCadastro
            };
            var vm = new ClienteEditViewModel(copy);

            bool isValid = false;
            while (!isValid)
            {
                var win = new ClientEditWindow() { DataContext = vm };
                if (win.ShowDialog() != true)
                {
                    return;
                }

                string? error = ValidateClient(copy, isEditing: true);
                if (error != null)
                {
                    MessageBox.Show(error, "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }

                isValid = true;
                try
                {
                    using var db = new Data.Database(MainViewModel.DbPath);
                    var sql = @"
                        UPDATE cliente SET tipo_pessoa=@TipoPessoa, nome_razao=@NomeRazao, documento=@Documento, 
                                           email=@Email, telefone=@Telefone 
                        WHERE id_cliente=@IdCliente;";
                    db.Connection.Execute(sql, copy);
                    var idx = Clientes.IndexOf(Selected);
                    if (idx != -1) Clientes[idx] = copy;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao atualizar cliente: {ex.Message}", "Erro de Banco de Dados", MessageBoxButton.OK, MessageBoxImage.Error);
                    isValid = false;
                }
            }
        }

        private void Delete()
        {
            if (MessageBox.Show($"Confirma a exclusão do cliente '{Selected.NomeRazao}'?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            try
            {
                using var db = new Data.Database(MainViewModel.DbPath);
                db.Connection.Execute("DELETE FROM cliente WHERE id_cliente=@IdCliente;", Selected);
                Clientes.Remove(Selected);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir cliente: {ex.Message}", "Erro de Banco de Dados", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanEditOrDelete() => Selected != null;

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(Selected))
            {
                (EditCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (DeleteCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }
    }
}
