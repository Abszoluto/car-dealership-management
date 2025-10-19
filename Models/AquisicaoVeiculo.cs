namespace CarDealerApp.Models
{
    public class AquisicaoVeiculo
    {
        public int IdAquisicao { get; set; }
        public int? IdVeiculo { get; set; }
        public string TipoOrigem { get; set; }
        public string FornecedorNome { get; set; }
        public string DataAquisicao { get; set; }
        public double? CustoAquisicao { get; set; }
        public string Documento { get; set; }
    }
}
