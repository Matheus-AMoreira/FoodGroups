using FoodGroups.Models;
using FoodGroups.Interfaces;

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

    public async Task<Usuario?> ObterPorId(int id)
    {
        return await _usuarioRepository.ProcurarUsuarioById(id);
    }

    public async Task Atualizar(int id, Usuario usuarioAtualizado)
    {
        var usuario = await _usuarioRepository.ProcurarUsuarioById(id);
        if (usuario == null) throw new Exception("Usuário não encontrado");

        // Atualiza campos permitidos
        usuario.Nome = usuarioAtualizado.Nome;
        usuario.Email = usuarioAtualizado.Email;
        // Nota: Senha idealmente deveria ter um fluxo separado de hash
        
        await _usuarioRepository.AtualizarUsuario(usuario);
    }

    public async Task Deletar(int id)
    {
        var usuario = await _usuarioRepository.ProcurarUsuarioById(id);
        if (usuario != null)
        {
            await _usuarioRepository.DeletarUsuario(usuario);
        }
    }
}
