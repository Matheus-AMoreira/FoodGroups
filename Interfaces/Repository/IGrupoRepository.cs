using FoodGroups.Models;

public interface IGrupoRepository
{
    Task CriarGrupo(Grupo grupo);
    Task UpdateGrupo(Grupo grupo);
    Task<Grupo?> GetGroupById(int id);
    Task<List<Grupo>> GetGruposWithUsuariosAndAgendas();
}
