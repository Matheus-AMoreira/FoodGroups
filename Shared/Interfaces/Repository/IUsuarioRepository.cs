using FoodGroups.Shared.Models;

namespace FoodGroups.Shared.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario> CriarUsuario(Usuario usuario);
    Task<Usuario?> ProcurarUsuarioById(int id);
    Task<List<Usuario>> ProcurarUsuariosByNameOrEmail(string termo);
    Task<List<Usuario>> ListarUsuarios();
    Task AtualizarUsuario(Usuario usuario);
    Task DeletarUsuario(Usuario usuario);
}
