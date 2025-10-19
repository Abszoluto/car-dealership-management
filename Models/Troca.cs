namespace CarDealerApp.Models
{
    public class Troca
    {
        public int IdTroca { get; set; }
        public int? IdVenda { get; set; }
        public string DescricaoVeiculoRecebido { get; set; }
        public double? ValorAvaliado { get; set; }
        public double? EstimativaPreparacao { get; set; }
    }
}
