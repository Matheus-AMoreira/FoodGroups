using FoodGroups.Models;
using Microsoft.EntityFrameworkCore;

public class GrupoRepository : IGrupoRepository
{
    private readonly AppDbContext _context;

    public GrupoRepository(AppDbContext context) => _context = context;

    public async Task<Usuario?> PostGrupo(int Id)
    {
        var result = await _context.Usuarios.FindAsync(Id);
        return result;
    }

    public async Task UpdateGrupo(Grupo grupo)
    {
        _context.Grupos.Update(grupo);
        await _context.SaveChangesAsync();
    }

    public async Task<Grupo?> GetGroupById(int id)
    {
        return await _context.Grupos.Include(g => g.Usuarios).FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<List<Grupo>> GetGruposWithUsuariosAndAgendas()
    {
        return await _context.Grupos
            .Include(g => g.Usuarios)
            .Include(g => g.Agendas)
            .ToListAsync();
    }
}
