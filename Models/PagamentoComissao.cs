namespace CarDealerApp.Models
{
    public class PagamentoComissao
    {
        public int IdPagamentoComissao { get; set; }
        public int? IdComissao { get; set; }
        public string DataPagamento { get; set; }
        public double? ValorPago { get; set; }
    }
}
