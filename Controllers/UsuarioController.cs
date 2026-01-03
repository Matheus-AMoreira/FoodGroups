using FoodGroups.Models;
using FoodGroups.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodGroups.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuarioController(UsuarioService service)
    {
        _service = service;
    }

    // GET: api/usuario
    [HttpGet]
    public async Task<ActionResult<List<Usuario>>> Get()
    {
        var usuarios = await _service.ListarTodos();
        return Ok(usuarios);
    }

    // GET: api/usuario/buscar?termo=joao
    [HttpGet("buscar")]
    public async Task<ActionResult<List<Usuario>>> Buscar([FromQuery] string termo)
    {
        var resultados = await _service.BuscarUsuarios(termo);
        return Ok(resultados);
    }

    // POST: api/usuario
    [HttpPost]
    public async Task<ActionResult<Usuario>> Post([FromBody] Usuario usuario)
    {
        var novoUsuario = await _service.CriarUsuario(usuario);
        return CreatedAtAction(nameof(Get), new { usuario = novoUsuario }, novoUsuario);
    }
}
