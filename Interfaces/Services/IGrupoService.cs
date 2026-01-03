using FoodGroups.DTOs;

public interface IGrupoService
{
    Task<String> AdicionarUsuario(int grupoId, int usuarioId, int solicitanteId);
    Task<Dictionary<string, List<ResumoRefeicaoDTO>>> ObterAgendaMensal(int? mes, int? ano);
}
