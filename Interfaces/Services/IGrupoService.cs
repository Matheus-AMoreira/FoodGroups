using FoodGroups.DTOs;
using FoodGroups.Models;

public interface IGrupoService
{
    Task<String> AdicionarUsuario(int grupoId, int usuarioId, int solicitanteId);
    Task<Dictionary<string, List<ResumoRefeicaoDTO>>> ObterAgendaMensal(int? mes, int? ano);
    Task<String> CriarGrupo(Grupo grupo);
}
