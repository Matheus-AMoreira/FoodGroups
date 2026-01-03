using FoodGroups.Models;

public interface IUsuarioService
{
    Task<String> CriarUsuario(Usuario usuario);
    Task<List<Usuario>> BuscarUsuarios(string termo);
    Task<List<Usuario>> ListarTodos();
}
