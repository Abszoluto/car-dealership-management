namespace CarDealerApp.Models
{
    public class ItemVenda
    {
        public int IdItemVenda { get; set; }
        public int? IdVenda { get; set; }
        public int? IdVeiculo { get; set; }
        public double? PrecoLista { get; set; }
        public double? Desconto { get; set; }
        public double? PrecoVenda { get; set; }
    }
}
