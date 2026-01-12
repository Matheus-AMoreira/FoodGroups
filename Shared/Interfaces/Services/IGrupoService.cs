using FoodGroups.Shared.DTOs;
using FoodGroups.Shared.Models;

namespace FoodGroups.Shared.Interfaces;

public interface IGrupoService
{
    Task<String> AdicionarUsuario(int grupoId, int usuarioId, int solicitanteId);
    Task<Dictionary<string, List<ResumoRefeicaoDTO>>> ObterAgendaMensal(int? mes, int? ano);
    Task<String> CriarGrupo(Grupo grupo);
    Task<List<Grupo>> ListarTodos();
    Task<Grupo?> ObterPorId(int id);
    Task Atualizar(int id, Grupo grupoInput);
    Task Deletar(int id);
}