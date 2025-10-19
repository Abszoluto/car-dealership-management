namespace CarDealerApp.Models
{
    public class ComissaoGerada
    {
        public int IdComissao { get; set; }
        public int? IdVenda { get; set; }
        public int? IdFuncionario { get; set; }
        public int? IdPlanoComissao { get; set; }
        public double? ValorBase { get; set; }
        public double? ValorComissao { get; set; }
        public string DataCalculo { get; set; }
    }
}
