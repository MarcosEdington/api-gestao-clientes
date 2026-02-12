namespace MercadinhoApi.Models
{
    public class Cliente
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty; 
        public string Telefone { get; set; } = string.Empty; 
        public decimal LimiteCredito { get; set; } = 1500.00m;
        public decimal SaldoUtilizado { get; set; } = 0;
        public bool Ativo { get; set; } = true; 
        public List<Compra> Historico { get; set; } = new List<Compra>();

        public decimal SaldoDisponivel => LimiteCredito - SaldoUtilizado;
    }
}
