using CommunityToolkit.Mvvm.ComponentModel;

namespace CarDealerApp.Models
{
    public class Veiculo : ObservableObject
    {
        public int IdVeiculo { get; set; }
        public int IdFilial { get; set; }
        public string Chassi { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int AnoModelo { get; set; }
        public double Valor { get; set; }
        public string Cor { get; set; }
        public int Quilometragem { get; set; }
        public string Combustivel { get; set; }
        public string Status { get; set; }
        public string DataCadastro { get; set; }
    }
}