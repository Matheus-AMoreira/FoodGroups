using FoodGroups.model;
using Microsoft.EntityFrameworkCore;

namespace FoodGroups.Service;

public class UsuarioService
{
    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context)
    {
        _context = context;
    }

    // Cria um novo usuário
    public async Task<Usuario> CriarUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    // Busca usuários por nome ou email (para o criador do grupo encontrar pessoas)
    public async Task<List<Usuario>> BuscarUsuarios(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return await _context.Usuarios.Take(10).ToListAsync();

        return await _context.Usuarios
            .Where(u => u.Nome.Contains(termo) || u.Email.Contains(termo))
            .ToListAsync();
    }

    // Listagem simples de todos
    public async Task<List<Usuario>> ListarTodos()
    {
        return await _context.Usuarios.ToListAsync();
    }
}
