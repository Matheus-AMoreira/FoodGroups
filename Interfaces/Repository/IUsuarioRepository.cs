using FoodGroups.Models;

public interface IUsuarioRepository
{
    Task CriarUsuario(Usuario usuario);
    Task<Usuario?> ProcurarUsuarioById(int Id);
    Task<List<Usuario>> ProcurarUsuariosByNameOrEmail(string termo);
    Task<List<Usuario>> ListarUsuarios();
}
