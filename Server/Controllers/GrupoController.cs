using FoodGroups.Shared.DTOs;
using FoodGroups.Shared.Models;
using FoodGroups.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodGroups.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GrupoController : ControllerBase
{
    private readonly IGrupoService _grupoService;
    public GrupoController(IGrupoService service) => _grupoService = service;

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        return Ok(await _grupoService.ListarTodos());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var grupo = await _grupoService.ObterPorId(id);
        if (grupo == null) return NotFound();
        return Ok(grupo);
    }

    [HttpPost]
    public async Task<IActionResult> CriarGrupo([FromBody] CriarGrupoDTO grupoDTO)
    {
        var grupo = new Grupo
        {
            Nome = grupoDTO.Nome,
            CapacidadeMaxima = grupoDTO.CapacidadeMaxima,
            CriadorId = grupoDTO.CriadorId,
            Agendas = grupoDTO.Agendas // O Service valida se tem itens aqui
        };

        var resultado = await _grupoService.CriarGrupo(grupo);
        
        if (resultado != "Grupo criado com sucesso")
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Grupo grupo)
    {
        if (id != grupo.Id) return BadRequest();
        try {
            await _grupoService.Atualizar(id, grupo);
            return NoContent();
        } catch {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        await _grupoService.Deletar(id);
        return NoContent();
    }

    // Endpoints específicos (Agenda, Adicionar Usuário) mantêm-se iguais
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
            await _grupoService.AdicionarUsuario(id, usuarioId, solicitanteId);
            return Ok("Usuário adicionado.");
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }
}
