using FoodGroups.Models;

namespace FoodGroups.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    // Cria um novo usuário
    public async Task<String> CriarUsuario(Usuario usuario)
    {
        await _usuarioRepository.CriarUsuario(usuario);
        return "Usuario adicionado";
    }

    // Busca usuários por nome ou email (para o criador do grupo encontrar pessoas)
    public async Task<List<Usuario>> BuscarUsuarios(string termo)
    {
        return await _usuarioRepository.ProcurarUsuariosByNameOrEmail(termo);
    }

    // Listagem simples de todos
    public async Task<List<Usuario>> ListarTodos()
    {
        return await _usuarioRepository.ListarUsuarios();
    }
}
