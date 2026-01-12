using FoodGroups.Shared.DTOs;
using FoodGroups.Shared.Models;

namespace FoodGroups.Shared.Interfaces;

public interface IGrupoService
{
    Task<String> CriarGrupo(Grupo grupo);
    public Task AdicionarUsuario(int grupoId, int usuarioId, int solicitanteId) => throw new NotImplementedException();
    public Task<List<Grupo>> ListarTodos();
    public Task<Grupo?> ObterPorId(int id);
    public Task Atualizar(int id, Grupo grupoInput) => throw new NotImplementedException();
    public Task Deletar(int id) => throw new NotImplementedException();
    public Task<Dictionary<string, List<ResumoRefeicaoDTO>>> ObterAgendaMensal(int? mes, int? ano);
}