using CommunityToolkit.Mvvm.ComponentModel;

namespace CarDealerApp.Models
{
    public class Filial : ObservableObject
    {
        private int _idFilial;
        public int IdFilial
        {
            get => _idFilial;
            set => SetProperty(ref _idFilial, value);
        }

        private string _nome;
        public string Nome
        {
            get => _nome;
            set => SetProperty(ref _nome, value);
        }

        private string _cnpj;
        public string Cnpj
        {
            get => _cnpj;
            set => SetProperty(ref _cnpj, value);
        }

        private string _estado;
        public string Estado
        {
            get => _estado;
            set => SetProperty(ref _estado, value);
        }

        private string _cidade;
        public string Cidade
        {
            get => _cidade;
            set => SetProperty(ref _cidade, value);
        }

        private string _dataCriacao;
        public string DataCriacao
        {
            get => _dataCriacao;
            set => SetProperty(ref _dataCriacao, value);
        }
    }
}
