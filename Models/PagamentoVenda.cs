namespace CarDealerApp.Models
{
    public class PagamentoVenda
    {
        public int IdPagamento { get; set; }
        public int? IdVenda { get; set; }
        public int? Parcela { get; set; }
        public string FormaPagamento { get; set; }
        public double? Valor { get; set; }
        public string DataPagamento { get; set; }
        public string Status { get; set; }
    }
}
