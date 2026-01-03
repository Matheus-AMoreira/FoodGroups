using FoodGroups.Models;

public interface IUsuarioRepository
{
    Task<Usuario?> GetUsuarioById(int id);
    Task<List<Usuario>> GetUsuarios();
    Task<List<Usuario>> GetUsuariosByNameOrEmail(string termo);
    Task PostUsuario(Usuario usuario);
}
