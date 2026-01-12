using FoodGroups.Shared.Models;

namespace FoodGroups.Shared.Interfaces;

public interface IGrupoRepository
{
    Task CriarGrupo(Grupo grupo);
    Task UpdateGrupo(Grupo grupo);
    Task<Grupo?> GetGroupById(int id);
    Task<List<Grupo>> GetGruposWithUsuariosAndAgendas();
    Task<List<Grupo>> ListarTodos();
    Task DeletarGrupo(Grupo grupo);
}
