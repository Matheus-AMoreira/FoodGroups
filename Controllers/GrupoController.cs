using Microsoft.AspNetCore.Mvc;

namespace FoodGroups.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GrupoController : ControllerBase
{
    private readonly IGrupoService _grupoService;
    public GrupoController(IGrupoService service) => _grupoService = service;

    [HttpGet("resumo-mensal")]
    public async Task<IActionResult> GetResumo([FromQuery] int? mes, [FromQuery] int? ano)
    {
        return Ok(await _grupoService.ObterAgendaMensal(mes, ano));
    }

    [HttpPost("{id}/adicionar-usuario")]
    public async Task<IActionResult> AddUser(int id, [FromBody] int usuarioId, [FromHeader] int solicitanteId)
    {
        try
        {
            String response = await _grupoService.AdicionarUsuario(id, usuarioId, solicitanteId);
            return Ok("Usuário adicionado e limite atualizado.");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
