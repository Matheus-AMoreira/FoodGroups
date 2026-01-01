using FoodGroups.Service;
using Microsoft.AspNetCore.Mvc;

namespace FoodGroups.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GrupoController : ControllerBase
{
    private readonly GrupoService _service;
    public GrupoController(GrupoService service) => _service = service;

    [HttpGet("resumo-mensal")]
    public async Task<IActionResult> GetResumo([FromQuery] int? mes, [FromQuery] int? ano)
    {
        return Ok(await _service.ObterAgendaMensal(mes, ano));
    }

    [HttpPost("{id}/adicionar-usuario")]
    public async Task<IActionResult> AddUser(int id, [FromBody] int usuarioId, [FromHeader] int solicitanteId)
    {
        try
        {
            await _service.AdicionarUsuario(id, usuarioId, solicitanteId);
            return Ok("Usuário adicionado e limite atualizado.");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
