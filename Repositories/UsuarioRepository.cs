using FoodGroups.Models;
using Microsoft.EntityFrameworkCore;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context) => _context = context;

    public async Task PostUsuario(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task<Usuario?> GetUsuarioById(int Id)
    {
        return await _context.Usuarios.FindAsync(Id);
    }

    public async Task<List<Usuario>> GetUsuariosByNameOrEmail(string termo)
    {
        return await _context.Usuarios
            .Where(u => u.Nome.Contains(termo) || u.Email.Contains(termo))
            .ToListAsync();
    }

    public async Task<List<Usuario>> GetUsuarios()
    {
        return await _context.Usuarios.ToListAsync();
    }
}
