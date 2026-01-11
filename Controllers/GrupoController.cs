using FoodGroups.DTOs;
using FoodGroups.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodGroups.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GrupoController : ControllerBase
{
    private readonly IGrupoService _grupoService;
    public GrupoController(IGrupoService service) => _grupoService = service;

    [HttpPost]
    public async Task<String> CriarGrupo(CriarGrupoDTO grupoDTO)
    {
        var grupo = new Grupo
        {
            Nome = grupoDTO.Nome,
            CapacidadeMaxima = grupoDTO.CapacidadeMaxima,
            CriadorId = grupoDTO.CriadorId,
            Usuarios = grupoDTO.Usuarios,
            Agendas = grupoDTO.Agendas
        };

        await _grupoService.CriarGrupo(grupo);

        return "oi";
    }

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
