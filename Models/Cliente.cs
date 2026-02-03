namespace MercadinhoApi.Models
{
    public class Cliente
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty; // Novo campo
        public string Telefone { get; set; } = string.Empty; // Novo campo
        public decimal LimiteCredito { get; set; } = 1500.00m;
        public decimal SaldoUtilizado { get; set; } = 0;
        public bool Ativo { get; set; } = true; // Para desativar em vez de excluir
        public List<Compra> Historico { get; set; } = new List<Compra>();

        public decimal SaldoDisponivel => LimiteCredito - SaldoUtilizado;
    }
}