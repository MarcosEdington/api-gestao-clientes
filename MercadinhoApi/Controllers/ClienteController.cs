using Microsoft.AspNetCore.Mvc;
using MercadinhoApi.Models;
using MercadinhoApi.Data;

namespace MercadinhoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        // Instanciamos o repositório que agora tem métodos específicos para Clientes e Usuários
        private readonly JsonRepository _repository = new JsonRepository();

        // 1. Listar todos os clientes cadastrados
        [HttpGet]
        public IActionResult Get()
        {
            var clientes = _repository.ObterTodosClientes();
            return Ok(clientes);
        }

        // 2. Buscar um cliente específico por ID (Útil para ver o extrato)
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var clientes = _repository.ObterTodosClientes();
            var cliente = clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null) return NotFound("Cliente não encontrado.");

            return Ok(cliente);
        }

        // 3. Cadastrar um novo cliente
        [HttpPost]
        public IActionResult Post([FromBody] Cliente novoCliente)
        {
            var clientes = _repository.ObterTodosClientes();

            // Regra: Não permitir duplicidade de nomes para evitar confusão no mercadinho
            if (clientes.Any(c => c.Nome.ToLower() == novoCliente.Nome.ToLower()))
            {
                return BadRequest("Já existe um cliente cadastrado com este nome.");
            }

            // Aplicando o novo limite de R$ 1.500,00 conforme sua solicitação
            novoCliente.LimiteCredito = 1500.00m;
            novoCliente.SaldoUtilizado = 0;
            novoCliente.Ativo = true; // Garante que ele nasce ativo
            novoCliente.Historico = new List<Compra>();

            clientes.Add(novoCliente);
            _repository.SalvarClientes(clientes);

            return Ok(novoCliente);
        }

        // 4. Registrar uma compra (A trava do Fiado)
        [HttpPost("{id}/venda")]
        public IActionResult RegistrarVenda(Guid id, [FromBody] Compra novaCompra)
        {
            var clientes = _repository.ObterTodosClientes();
            var cliente = clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null) return NotFound("Cliente não encontrado.");

            // Validação de valor negativo (Segurança extra)
            if (novaCompra.Valor <= 0)
            {
                return BadRequest("O valor da compra deve ser maior que zero.");
            }

            // --- REGRA DE OURO DO MERCADINHO ---
            // Verifica se o valor da compra ultrapassa o saldo disponível
            if (novaCompra.Valor > cliente.SaldoDisponivel)
            {
                return BadRequest($"Venda Bloqueada! Limite insuficiente. " +
                                 $"O cliente {cliente.Nome} possui apenas R$ {cliente.SaldoDisponivel} disponível.");
            }

            // Se estiver tudo OK, atualiza os dados do cliente
            cliente.SaldoUtilizado += novaCompra.Valor;

            // Adiciona a data atual se não vier informada
            novaCompra.Data = DateTime.Now;
            cliente.Historico.Add(novaCompra);

            // Salva a lista atualizada no arquivo clientes.json
            _repository.SalvarClientes(clientes);

            return Ok(new
            {
                Mensagem = "Venda realizada com sucesso!",
                Cliente = cliente.Nome,
                ValorGasto = novaCompra.Valor,
                SaldoRestante = cliente.SaldoDisponivel
            });
        }


        //[HttpPost("{id}/pagamento")]
        //public IActionResult RegistrarPagamento(Guid id, [FromQuery] decimal valorPago)
        //{
        //    var clientes = _repository.ObterTodosClientes();
        //    var cliente = clientes.FirstOrDefault(c => c.Id == id);

        //    if (cliente == null) return NotFound("Cliente não encontrado.");
        //    if (valorPago <= 0) return BadRequest("O valor do pagamento deve ser positivo.");

        //    cliente.SaldoUtilizado -= valorPago;
        //    if (cliente.SaldoUtilizado < 0) cliente.SaldoUtilizado = 0;

        //    // Basta criar a nova compra. O ID, a Data e a Descrição padrão 
        //    // já são tratados pelo Modelo ou por este bloco:
        //    cliente.Historico.Add(new Compra
        //    {
        //        // NÃO COLOQUE O CAMPO ID AQUI. 
        //        // O modelo Compra.cs já gera um Guid.NewGuid() sozinho.
        //        Descricao = "PAGAMENTO REALIZADO",
        //        Valor = -valorPago,
        //        Data = DateTime.Now
        //    });

        //    _repository.SalvarClientes(clientes);
        //    return Ok("Pagamento registrado com sucesso!");
        //}

        [HttpPost("{id}/pagamento")]
        public IActionResult RegistrarPagamento(Guid id, [FromQuery] decimal valorPago)
        {
            var clientes = _repository.ObterTodosClientes();
            var cliente = clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null) return NotFound("Cliente não encontrado.");
            if (valorPago <= 0) return BadRequest("O valor do pagamento deve ser positivo.");

            // REMOVEMOS A TRAVA! Agora o SaldoUtilizado pode ser -12.50 (Crédito)
            cliente.SaldoUtilizado -= valorPago;

            cliente.Historico.Add(new Compra
            {
                Descricao = "PAGAMENTO REALIZADO",
                Valor = -valorPago, // Mantemos negativo para o cálculo matemático bater
                Data = DateTime.Now
            });

            _repository.SalvarClientes(clientes);
            return Ok(new { mensagem = "Pagamento registrado com sucesso!", novoSaldo = cliente.SaldoUtilizado });
        }


        // MÉTODO CORRIGIDO PARA JSON REPOSITORY
        [HttpDelete("{id}")]
        public IActionResult Desativar(Guid id)
        {
            var clientes = _repository.ObterTodosClientes();
            var cliente = clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null) return NotFound("Cliente não encontrado.");

            // Em vez de deletar o arquivo, apenas marcamos como inativo
            cliente.Ativo = false;

            _repository.SalvarClientes(clientes);

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Cliente clienteAtualizado)
        {
            var clientes = _repository.ObterTodosClientes();
            var cliente = clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null) return NotFound("Cliente não encontrado.");

            // Atualiza apenas os campos permitidos
            cliente.Nome = clienteAtualizado.Nome;
            cliente.CPF = clienteAtualizado.CPF;
            cliente.Telefone = clienteAtualizado.Telefone;
            cliente.Ativo = clienteAtualizado.Ativo; // <--- ADICIONE ESTA LINHA AQUI!

            _repository.SalvarClientes(clientes);

            return NoContent();
        }

    }
}