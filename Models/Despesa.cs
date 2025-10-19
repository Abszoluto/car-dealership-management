namespace CarDealerApp.Models
{
    public class Despesa
    {
        public int IdDespesa { get; set; }
        public int? IdFilial { get; set; }
        public int? IdVeiculo { get; set; }
        public int? IdFuncionario { get; set; }
        public string TipoDespesa { get; set; }
        public string Fornecedor { get; set; }
        public string Descricao { get; set; }
        public double? Valor { get; set; }
        public string MesReferencia { get; set; }
        public string DataDespesa { get; set; }
        public string Documento { get; set; }
    }
}
