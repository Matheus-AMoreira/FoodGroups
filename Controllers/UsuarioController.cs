using FoodGroups.Models;
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
    public async Task<ActionResult<List<Usuario>>> Get()
    {
        var usuarios = await _usuarioService.ListarTodos();
        return Ok(usuarios);
    }

    // GET: api/usuario/buscar?termo=joao
    [HttpGet("buscar")]
    public async Task<ActionResult<List<Usuario>>> Buscar([FromQuery] string termo)
    {
        var resultados = await _usuarioService.BuscarUsuarios(termo);
        return Ok(resultados);
    }

    // POST: api/usuario
    [HttpPost]
    public async Task<ActionResult<Usuario>> Post([FromBody] Usuario usuario)
    {
        var novoUsuario = await _usuarioService.CriarUsuario(usuario);
        return CreatedAtAction(nameof(Get), new { usuario = novoUsuario }, novoUsuario);
    }
}
