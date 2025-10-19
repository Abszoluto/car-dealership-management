namespace CarDealerApp.Models
{
    public class PlanoComissao
    {
        public int IdPlanoComissao { get; set; }
        public string Nome { get; set; }
        public string BaseCalculo { get; set; }
        public double? Percentual { get; set; }
        public double? ValorFixo { get; set; }
        public double? PisoMargem { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
    }
}
