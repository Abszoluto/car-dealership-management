namespace CarDealerApp.Models
{
    public class PedidoVenda
    {
        public int IdVenda { get; set; }
        public int? IdFilial { get; set; }
        public int? IdCliente { get; set; }
        public int? IdVendedor { get; set; }
        public string Status { get; set; }
        public string DataVenda { get; set; }
        public string DataCriacao { get; set; }
        public string DataCancelamento { get; set; }
    }
}
