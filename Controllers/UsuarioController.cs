using FoodGroups.Models;
using FoodGroups.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FoodGroups.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    public UsuarioController(IUsuarioService service) => _usuarioService = service;

    // GET: api/usuario
    [HttpGet]
    public async Task<ActionResult<List<Usuario>>> GetUsuarios()
    {
        var usuarios = await _usuarioService.ListarTodos();
        return Ok(usuarios);
    }

    // GET: api/usuario/buscar?termo=joao
    [HttpGet("buscar")]
    public async Task<ActionResult<List<Usuario>>> BuscarUsuarios([FromQuery] string termo)
    {
        var resultados = await _usuarioService.BuscarUsuarios(termo);
        return Ok(resultados);
    }

    // POST: api/usuario
    [HttpPost]
    public async Task<String> PostUsuario([FromBody] CriarUsuarioDTO usuarioDTO)
    {
        var usuario = new Usuario
        {
            Nome = usuarioDTO.Nome,
            Email = usuarioDTO.Email,
            Senha = usuarioDTO.Senha
        };

        var resultado = await _usuarioService.CriarUsuario(usuario);

        return resultado;
    }
}
