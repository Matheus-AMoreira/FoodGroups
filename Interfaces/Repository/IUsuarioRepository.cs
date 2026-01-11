using FoodGroups.Models;

public interface IUsuarioRepository
{
    Task<Usuario> CriarUsuario(Usuario usuario);
    Task<Usuario?> ProcurarUsuarioById(int id);
    Task<List<Usuario>> ProcurarUsuariosByNameOrEmail(string termo);
    Task<List<Usuario>> ListarUsuarios();
}
