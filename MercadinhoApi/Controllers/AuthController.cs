using Microsoft.AspNetCore.Mvc;
using MercadinhoApi.Models;
using MercadinhoApi.Data;

namespace MercadinhoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JsonRepository _repository = new JsonRepository();

        [HttpPost("login")]
        public IActionResult Login([FromBody] Usuario loginRequest)
        {
            var usuarios = _repository.ObterTodosUsuarios();

            // Busca o usuário no JSON pelo email e senha
            var usuario = usuarios.FirstOrDefault(u =>
                u.Email == loginRequest.Email && u.Senha == loginRequest.Senha);

            if (usuario == null)
            {
                return Unauthorized("E-mail ou senha inválidos.");
            }

            return Ok(new
            {
                Mensagem = "Bem-vindo!",
                Usuario = usuario.Nome,
                Perfil = usuario.Perfil
            });
        }

        // Método para você cadastrar o primeiro administrador via Swagger
        [HttpPost("registrar")]
        public IActionResult Registrar([FromBody] Usuario novoUsuario)
        {
            var usuarios = _repository.ObterTodosUsuarios();

            // Validação: Verifica se já existe alguém com esse e-mail
            if (usuarios.Any(u => u.Email == novoUsuario.Email))
            {
                return BadRequest("Este e-mail já está cadastrado.");
            }

            usuarios.Add(novoUsuario);
            _repository.SalvarUsuarios(usuarios);
            return Ok("Usuário criado com sucesso!");
        }
    }
}