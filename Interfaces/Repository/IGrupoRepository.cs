using FoodGroups.Models;

public interface IGrupoRepository
{
    Task<Usuario?> PostGrupo(int Id);
    Task UpdateGrupo(Grupo grupo);
    Task<Grupo?> GetGroupById(int id);
    Task<List<Grupo>> GetGruposWithUsuariosAndAgendas();
}
