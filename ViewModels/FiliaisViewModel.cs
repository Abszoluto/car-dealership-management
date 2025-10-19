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

namespace CarDealerApp.ViewModels
{
    public class FiliaisViewModel : ObservableObject
    {
        public ObservableCollection<Filial> Filiais { get; set; }

        private Filial _selectedFilial;
        public Filial SelectedFilial
        {
            get => _selectedFilial;
            set => SetProperty(ref _selectedFilial, value);
        }


        public IRelayCommand AddFilialCommand { get; }
        public IRelayCommand EditFilialCommand { get; }
        public IRelayCommand DeleteFilialCommand { get; }
        public IRelayCommand RefreshCommand { get; }

        public FiliaisViewModel()
        {
            Filiais = new ObservableCollection<Filial>();

            AddFilialCommand = new RelayCommand(AddFilial);
            EditFilialCommand = new RelayCommand(EditFilial, CanEditOrDeleteFilial);
            DeleteFilialCommand = new RelayCommand(DeleteFilial, CanEditOrDeleteFilial);
            RefreshCommand = new RelayCommand(LoadFiliais);


            LoadFiliais();
        }

        public void LoadFiliais()
        {
            Filiais.Clear();
            try
            {
                // Abre a conexão com o banco de dados e carrega as filiais
                using var db = new Data.Database(MainViewModel.DbPath);

                // Usar aliases de coluna explícitos para garantir o mapeamento correto pelo Dapper.
                var query = @"
                    SELECT 
                        id_filial AS IdFilial, 
                        nome AS Nome, 
                        cnpj AS Cnpj, 
                        estado AS Estado, 
                        cidade AS Cidade, 
                        data_criacao AS DataCriacao 
                    FROM filial;";

                var filiaisFromDb = db.Connection.Query<Filial>(query).ToList();
                foreach (var filial in filiaisFromDb)
                {
                    Filiais.Add(filial);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar filiais do banco de dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddFilial()
        {
            var newFilial = new Filial();
            var vm = new FilialEditViewModel(newFilial);

            bool isDataValid = false;
            while (!isDataValid)
            {
                var window = new FilialEditWindow { DataContext = vm };
                if (window.ShowDialog() != true)
                {
                    return;
                }


                var cnpjLimpo = Regex.Replace(newFilial.Cnpj ?? "", @"[^\d]", "");
                if (cnpjLimpo.Length != 14)
                {
                    MessageBox.Show("O CNPJ deve conter 14 dígitos.", "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Error);

                    continue;
                }

                try
                {
                    using (var db = new Data.Database(MainViewModel.DbPath))
                    {
                        var existe = db.Connection.ExecuteScalar<bool>("SELECT COUNT(1) FROM filial WHERE cnpj = @Cnpj", new { Cnpj = newFilial.Cnpj });
                        if (existe)
                        {
                            MessageBox.Show("Este CNPJ já está cadastrado.", "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Error);
                            continue;
                        }

                        isDataValid = true;

                        newFilial.DataCriacao = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var id = db.Connection.ExecuteScalar<long>(@"
                            INSERT INTO filial (nome, cnpj, estado, cidade, data_criacao)
                            VALUES (@Nome, @Cnpj, @Estado, @Cidade, @DataCriacao);
                            SELECT last_insert_rowid();", newFilial);

                        newFilial.IdFilial = (int)id;
                        Filiais.Add(newFilial);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao salvar nova filial: {ex.Message}", "Erro de Banco de Dados", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void EditFilial()
        {
            var filialToEdit = new Filial
            {
                IdFilial = SelectedFilial.IdFilial,
                Nome = SelectedFilial.Nome,
                Cnpj = SelectedFilial.Cnpj,
                Estado = SelectedFilial.Estado,
                Cidade = SelectedFilial.Cidade,
                DataCriacao = SelectedFilial.DataCriacao
            };

            var vm = new FilialEditViewModel(filialToEdit);

            bool isDataValid = false;
            while (!isDataValid)
            {
                var window = new FilialEditWindow { DataContext = vm };
                if (window.ShowDialog() != true)
                {
                    return;
                }

                // Validação do formato do CNPJ
                var cnpjLimpo = Regex.Replace(filialToEdit.Cnpj ?? "", @"[^\d]", "");
                if (cnpjLimpo.Length != 14)
                {
                    MessageBox.Show("O CNPJ deve conter 14 dígitos.", "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }

                try
                {
                    using (var db = new Data.Database(MainViewModel.DbPath))
                    {
                        // Validação de unicidade do CNPJ
                        var existe = db.Connection.ExecuteScalar<bool>("SELECT COUNT(1) FROM filial WHERE cnpj = @Cnpj AND id_filial != @IdFilial", new { Cnpj = filialToEdit.Cnpj, IdFilial = filialToEdit.IdFilial });
                        if (existe)
                        {
                            MessageBox.Show("Este CNPJ já está cadastrado para outra filial.", "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Error);
                            continue;
                        }

                        isDataValid = true;

                        var affectedRows = db.Connection.Execute(@"
                            UPDATE filial SET 
                                nome = @Nome, 
                                cnpj = @Cnpj, 
                                estado = @Estado, 
                                cidade = @Cidade 
                            WHERE id_filial = @IdFilial;", filialToEdit);

                        if (affectedRows > 0)
                        {
                            var index = Filiais.IndexOf(SelectedFilial);
                            if (index != -1)
                            {
                                Filiais[index] = filialToEdit;
                            }
                        }
                        else
                        {
                            MessageBox.Show("A atualização falhou. O registo não foi encontrado no banco de dados.", "Erro de Atualização", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao atualizar filial: {ex.Message}", "Erro de Banco de Dados", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void DeleteFilial()
        {
            if (MessageBox.Show($"Tem certeza que deseja excluir a filial '{SelectedFilial.Nome}'?",
                                "Confirmar Exclusão",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    using var db = new Data.Database(MainViewModel.DbPath);
                    var affectedRows = db.Connection.Execute("DELETE FROM filial WHERE id_filial = @IdFilial;", SelectedFilial);

                    if (affectedRows > 0)
                    {
                        Filiais.Remove(SelectedFilial);
                    }
                    else
                    {
                        MessageBox.Show("A exclusão falhou. O registo não foi encontrado no banco de dados.", "Erro de Exclusão", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir filial: {ex.Message}", "Erro de Banco de Dados", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanEditOrDeleteFilial()
        {
            return SelectedFilial != null;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedFilial))
            {
                EditFilialCommand.NotifyCanExecuteChanged();
                DeleteFilialCommand.NotifyCanExecuteChanged();
            }
        }
    }
}
