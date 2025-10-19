using CarDealerApp.Models;
using CarDealerApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dapper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace CarDealerApp.ViewModels
{
    public class VehiclesViewModel : ObservableObject
    {
        public ObservableCollection<Veiculo> Veiculos { get; set; }
        public ObservableCollection<Filial> FiliaisDisponiveis { get; set; }

        private Veiculo _selectedVeiculo;
        public Veiculo SelectedVeiculo
        {
            get => _selectedVeiculo;
            set => SetProperty(ref _selectedVeiculo, value);
        }

        public IRelayCommand AddVehicleCommand { get; }
        public IRelayCommand EditVehicleCommand { get; }
        public IRelayCommand DeleteVehicleCommand { get; }
        public IRelayCommand RefreshCommand { get; }

        public VehiclesViewModel()
        {
            Veiculos = new ObservableCollection<Veiculo>();
            FiliaisDisponiveis = new ObservableCollection<Filial>();

            AddVehicleCommand = new RelayCommand(AddVehicle);
            EditVehicleCommand = new RelayCommand(EditVehicle, CanEditOrDelete);
            DeleteVehicleCommand = new RelayCommand(DeleteVehicle, CanEditOrDelete);
            RefreshCommand = new RelayCommand(LoadData);

            LoadData();
        }

        private void LoadData()
        {
            LoadFiliais();
            LoadVeiculos();
        }

        private void LoadVeiculos()
        {
            Veiculos.Clear();
            try
            {
                using var db = new Data.Database(MainViewModel.DbPath);
                var query = @"
                    SELECT 
                        id_veiculo AS IdVeiculo, id_filial AS IdFilial, chassi AS Chassi, 
                        marca AS Marca, modelo AS Modelo, ano_modelo AS AnoModelo, valor AS Valor, 
                        cor AS Cor, quilometragem AS Quilometragem, combustivel AS Combustivel, status AS Status,
                        data_cadastro as DataCadastro
                    FROM veiculo;";
                var veiculosFromDb = db.Connection.Query<Veiculo>(query).ToList();
                foreach (var veiculo in veiculosFromDb)
                {
                    Veiculos.Add(veiculo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar veículos: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadFiliais()
        {
            FiliaisDisponiveis.Clear();
            try
            {
                using var db = new Data.Database(MainViewModel.DbPath);
                var query = "SELECT id_filial AS IdFilial, nome AS Nome FROM filial;";
                var filiaisFromDb = db.Connection.Query<Filial>(query).ToList();
                foreach (var filial in filiaisFromDb)
                {
                    FiliaisDisponiveis.Add(filial);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar filiais: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string? ValidateVehicle(Veiculo vehicle)
        {
            if (vehicle.IdFilial == 0)
                return "Por favor, selecione uma filial.";

            if (string.IsNullOrWhiteSpace(vehicle.Chassi))
                return "O campo Chassi é obrigatório.";

            if (string.IsNullOrWhiteSpace(vehicle.Marca))
                return "O campo Marca é obrigatório.";

            if (string.IsNullOrWhiteSpace(vehicle.Modelo))
                return "O campo Modelo é obrigatório.";

            if (vehicle.AnoModelo < 1950 || vehicle.AnoModelo > DateTime.Now.Year + 1)
                return $"O Ano do Modelo deve ser entre 1950 e {DateTime.Now.Year + 1}.";


            if (string.IsNullOrWhiteSpace(vehicle.Cor))
                return "O campo Cor é obrigatório.";

            if (vehicle.Quilometragem < 0)
                return "A Quilometragem não pode ser negativa.";

            if (string.IsNullOrWhiteSpace(vehicle.Combustivel))
                return "O campo Tipo de Combustível é obrigatório.";

            if (vehicle.Valor <= 0)
                return "O Valor do veículo deve ser maior que zero.";

            return null;
        }

        private void AddVehicle()
        {
            LoadFiliais();

            var newVehicle = new Veiculo() { Status = "estoque" };
            var vm = new VeiculoEditViewModel(newVehicle, FiliaisDisponiveis);

            bool isDataValid = false;
            while (!isDataValid)
            {
                var window = new VehicleEditWindow { DataContext = vm };
                if (window.ShowDialog() != true)
                {
                    return;
                }

                string? validationError = ValidateVehicle(newVehicle);
                if (validationError != null)
                {
                    MessageBox.Show(validationError, "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }

                isDataValid = true;

                try
                {
                    newVehicle.DataCadastro = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    using var db = new Data.Database(MainViewModel.DbPath);
                    var sql = @"
                        INSERT INTO veiculo (id_filial, chassi, marca, modelo, ano_modelo, valor, cor, quilometragem, combustivel, status, data_cadastro)
                        VALUES (@IdFilial, @Chassi, @Marca, @Modelo, @AnoModelo, @Valor, @Cor, @Quilometragem, @Combustivel, @Status, @DataCadastro);
                        SELECT last_insert_rowid();";

                    var id = db.Connection.ExecuteScalar<long>(sql, newVehicle);

                    newVehicle.IdVeiculo = (int)id;
                    Veiculos.Add(newVehicle);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao salvar novo veículo: {ex.Message}", "Erro de Banco de Dados", MessageBoxButton.OK, MessageBoxImage.Error);
                    isDataValid = false;
                }
            }
        }

        private void EditVehicle()
        {
            LoadFiliais();

            var vehicleToEdit = new Veiculo
            {
                IdVeiculo = SelectedVeiculo.IdVeiculo,
                IdFilial = SelectedVeiculo.IdFilial,
                Chassi = SelectedVeiculo.Chassi,
                Marca = SelectedVeiculo.Marca,
                Modelo = SelectedVeiculo.Modelo,
                AnoModelo = SelectedVeiculo.AnoModelo,
                Valor = SelectedVeiculo.Valor,
                Cor = SelectedVeiculo.Cor,
                Quilometragem = SelectedVeiculo.Quilometragem,
                Combustivel = SelectedVeiculo.Combustivel,
                Status = SelectedVeiculo.Status,
                DataCadastro = SelectedVeiculo.DataCadastro
            };

            var vm = new VeiculoEditViewModel(vehicleToEdit, FiliaisDisponiveis);

            bool isDataValid = false;
            while (!isDataValid)
            {
                var window = new VehicleEditWindow { DataContext = vm };
                if (window.ShowDialog() != true)
                {
                    return;
                }

                string? validationError = ValidateVehicle(vehicleToEdit);
                if (validationError != null)
                {
                    MessageBox.Show(validationError, "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }

                isDataValid = true;

                try
                {
                    using var db = new Data.Database(MainViewModel.DbPath);
                    db.Connection.Execute(@"
                        UPDATE veiculo SET 
                            id_filial = @IdFilial, chassi = @Chassi, marca = @Marca, modelo = @Modelo, 
                            ano_modelo = @AnoModelo, valor = @Valor, cor = @Cor, 
                            quilometragem = @Quilometragem, combustivel = @Combustivel, status = @Status 
                        WHERE id_veiculo = @IdVeiculo;", vehicleToEdit);

                    var index = Veiculos.IndexOf(SelectedVeiculo);
                    if (index != -1) Veiculos[index] = vehicleToEdit;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao atualizar veículo: {ex.Message}", "Erro de Banco de Dados", MessageBoxButton.OK, MessageBoxImage.Error);
                    isDataValid = false;
                }
            }
        }

        private void DeleteVehicle()
        {
            if (MessageBox.Show($"Confirma exclusão do veículo {SelectedVeiculo.Marca} {SelectedVeiculo.Modelo}?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    using var db = new Data.Database(MainViewModel.DbPath);
                    db.Connection.Execute("DELETE FROM veiculo WHERE id_veiculo = @IdVeiculo;", SelectedVeiculo);
                    Veiculos.Remove(SelectedVeiculo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir veículo: {ex.Message}", "Erro de Banco de Dados", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanEditOrDelete() => SelectedVeiculo != null;

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedVeiculo))
            {
                EditVehicleCommand.NotifyCanExecuteChanged();
                DeleteVehicleCommand.NotifyCanExecuteChanged();
            }
        }
    }
}
