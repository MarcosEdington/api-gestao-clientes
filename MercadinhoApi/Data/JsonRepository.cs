using System.Text.Json;
using MercadinhoApi.Models;

namespace MercadinhoApi.Data
{
    public class JsonRepository
    {
        // Caminhos diferentes para arquivos diferentes
        private readonly string _caminhoClientes = "clientes.json";
        private readonly string _caminhoUsuarios = "usuarios.json";

        // ================= SERVIÇOS DE CLIENTES =================
        public List<Cliente> ObterTodosClientes()
        {
            if (!File.Exists(_caminhoClientes)) return new List<Cliente>();
            var json = File.ReadAllText(_caminhoClientes);
            return JsonSerializer.Deserialize<List<Cliente>>(json) ?? new List<Cliente>();
        }

        public void SalvarClientes(List<Cliente> clientes)
        {
            var json = JsonSerializer.Serialize(clientes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_caminhoClientes, json);
        }

        // ================= SERVIÇOS DE USUÁRIOS =================
        public List<Usuario> ObterTodosUsuarios()
        {
            if (!File.Exists(_caminhoUsuarios)) return new List<Usuario>();
            var json = File.ReadAllText(_caminhoUsuarios);
            return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }

        public void SalvarUsuarios(List<Usuario> usuarios)
        {
            var json = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_caminhoUsuarios, json);
        }
    }
}