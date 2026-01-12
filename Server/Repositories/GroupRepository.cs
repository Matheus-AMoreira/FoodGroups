using FoodGroups.Shared.Models;
using FoodGroups.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

public class GrupoRepository : IGrupoRepository
{
    private readonly AppDbContext _context;

    public GrupoRepository(AppDbContext context) => _context = context;

        public async Task<List<Grupo>> GetGruposWithUsuariosAndAgendas()
    {
        return await _context.Grupos
            .Include(g => g.Usuarios)
            .Include(g => g.Agendas)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<List<Grupo>> ListarTodos()
    {
        return await _context.Grupos
            .Include(g => g.Usuarios)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<Grupo?> GetGroupById(int id)
    {
        return await _context.Grupos
            .Include(g => g.Usuarios)
            .Include(g => g.Agendas)
            .AsSplitQuery()
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task CriarGrupo(Grupo grupo)
    {
        _context.Grupos.Add(grupo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateGrupo(Grupo grupo)
    {
        _context.Grupos.Update(grupo);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarGrupo(Grupo grupo)
    {
        _context.Grupos.Remove(grupo);
        await _context.SaveChangesAsync();
    }
    
}
