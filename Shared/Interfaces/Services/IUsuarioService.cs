using FoodGroups.Shared.Models;

namespace FoodGroups.Shared.Interfaces;

public interface IUsuarioService
{
    Task<String> CriarUsuario(Usuario usuario);
    Task<List<Usuario>> BuscarUsuarios(string termo);
    Task<List<Usuario>> ListarTodos();
    Task<Usuario?> ObterPorId(int id);
    Task Atualizar(int id, Usuario usuarioAtualizado);
    Task Deletar(int id);
}
