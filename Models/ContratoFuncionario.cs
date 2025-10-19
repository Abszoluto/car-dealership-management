namespace CarDealerApp.Models
{
    public class ContratoFuncionario
    {
        public int IdContrato { get; set; }
        public int? IdFuncionario { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        public double? SalarioBase { get; set; }
        public int? IdPlanoComissao { get; set; }
    }
}
