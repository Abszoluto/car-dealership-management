namespace CarDealerApp.Models
{
    public class Funcionario
    {
        public int IdFuncionario { get; set; }
        public int? IdFilial { get; set; }
        public string NomeCompleto { get; set; }
        public string Cargo { get; set; }
        public string DataAdmissao { get; set; }
        public string DataDemissao { get; set; }
        public int? Ativo { get; set; }
    }
}
