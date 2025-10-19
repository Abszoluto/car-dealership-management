using CommunityToolkit.Mvvm.ComponentModel;

namespace CarDealerApp.Models
{
    public class Cliente : ObservableObject
    {
        private int _idCliente;
        public int IdCliente
        {
            get => _idCliente;
            set => SetProperty(ref _idCliente, value);
        }

        private string _tipoPessoa;
        public string TipoPessoa
        {
            get => _tipoPessoa;
            set => SetProperty(ref _tipoPessoa, value);
        }

        private string _nomeRazao;
        public string NomeRazao
        {
            get => _nomeRazao;
            set => SetProperty(ref _nomeRazao, value);
        }

        private string _documento;
        public string Documento
        {
            get => _documento;
            set => SetProperty(ref _documento, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _telefone;
        public string Telefone
        {
            get => _telefone;
            set => SetProperty(ref _telefone, value);
        }

        private string _dataCadastro;
        public string DataCadastro
        {
            get => _dataCadastro;
            set => SetProperty(ref _dataCadastro, value);
        }
    }
}
