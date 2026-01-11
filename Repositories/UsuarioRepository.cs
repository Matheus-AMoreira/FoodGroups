using FoodGroups.Models;
using Microsoft.EntityFrameworkCore;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context) => _context = context;

    public async Task<Usuario> CriarUsuario(Usuario usuario)
    {
        var entry = _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        
        return entry.Entity;
    }

    public async Task<Usuario?> ProcurarUsuarioById(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<List<Usuario>> ProcurarUsuariosByNameOrEmail(string termo)
    {
        return await _context.Usuarios
            .Where(u => u.Nome.Contains(termo) || u.Email.Contains(termo))
            .ToListAsync();
    }

    public async Task<List<Usuario>> ListarUsuarios()
    {
        return await _context.Usuarios.ToListAsync();
    }
}
