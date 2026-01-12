using FoodGroups.Models;
using FoodGroups.DTOs;
using FoodGroups.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodGroups.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    public UsuarioController(IUsuarioService service) => _usuarioService = service;

    [HttpGet]
    public async Task<ActionResult<List<Usuario>>> GetUsuarios()
    {
        return Ok(await _usuarioService.ListarTodos());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetById(int id)
    {
        var usuario = await _usuarioService.ObterPorId(id);
        if (usuario == null) return NotFound();
        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> PostUsuario([FromBody] CriarUsuarioDTO usuarioDTO)
    {
        var usuario = new Usuario
        {
            Nome = usuarioDTO.Nome,
            Email = usuarioDTO.Email,
            Senha = usuarioDTO.Senha // Lembre-se de hashear a senha em produção
        };
        await _usuarioService.CriarUsuario(usuario);
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuario(int id, [FromBody] Usuario usuario)
    {
        if (id != usuario.Id) return BadRequest();
        await _usuarioService.Atualizar(id, usuario);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        await _usuarioService.Deletar(id);
        return NoContent();
    }
    
    [HttpGet("buscar")]
    public async Task<ActionResult<List<Usuario>>> Buscar([FromQuery] string termo)
    {
        return Ok(await _usuarioService.BuscarUsuarios(termo));
    }
}
