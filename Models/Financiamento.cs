namespace CarDealerApp.Models
{
    public class Financiamento
    {
        public int IdFinanciamento { get; set; }
        public int? IdVenda { get; set; }
        public string Banco { get; set; }
        public double? ValorFinanciado { get; set; }
        public double? TaxaJurosAnual { get; set; }
        public string DataAprovacao { get; set; }
    }
}
