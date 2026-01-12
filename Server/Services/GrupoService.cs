using FoodGroups.Shared.Models;
using FoodGroups.Shared.DTOs;
using FoodGroups.Shared.Interfaces;

namespace FoodGroups.Services;

public class GrupoService : IGrupoService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IGrupoRepository _grupoRepository;

    public GrupoService
    (
        IUsuarioRepository usuarioRepository,
        IGrupoRepository grupoRepository
    )
    {
        _usuarioRepository = usuarioRepository;
        _grupoRepository = grupoRepository;
    }

    public async Task<String> CriarGrupo(Grupo grupo)
    {
        // 1. Validação de Dono
        if (grupo.CriadorId <= 0)
        {
             return "O grupo precisa ter um criador (Dono) vinculado.";
        }

        // 2. Validação de Agenda (Regra solicitada)
        if (grupo.Agendas == null || !grupo.Agendas.Any())
        {
            return "O grupo precisa ter pelo menos uma agenda definida (Ex: Almoço toda Segunda).";
        }

        if (grupo.CapacidadeMaxima <= 1) 
        {
            return "Capacidade precisa ser maior que 1";
        }

        // Verifica se o usuário criador existe
        var criador = await _usuarioRepository.ProcurarUsuarioById(grupo.CriadorId);
        if (criador == null) return "Usuário criador não encontrado.";

        // Adiciona o criador automaticamente à lista de usuários do grupo
        if (grupo.Usuarios == null) grupo.Usuarios = new List<Usuario>();
        grupo.Usuarios.Add(criador);

        await _grupoRepository.CriarGrupo(grupo);

        return "Grupo criado com sucesso";
    }

    public async Task<List<Grupo>> ListarTodos()
    {
        return await _grupoRepository.ListarTodos();
    }

    public async Task<Grupo?> ObterPorId(int id)
    {
        return await _grupoRepository.GetGroupById(id);
    }

    public async Task Atualizar(int id, Grupo grupoInput)
    {
        var grupoDb = await _grupoRepository.GetGroupById(id);
        if (grupoDb == null) throw new Exception("Grupo não encontrado");

        // Atualiza dados básicos
        grupoDb.Nome = grupoInput.Nome;
        grupoDb.CapacidadeMaxima = grupoInput.CapacidadeMaxima;
        
        // Atualiza Agendas (Remove antigas e adiciona novas)
        grupoDb.Agendas.Clear();
        foreach (var agenda in grupoInput.Agendas)
        {
            // Garante que o ID seja resetado para criar novas entradas se necessário
            // ou mantém se o EF for inteligente o suficiente, mas Clear() + Add costuma exigir reset
            agenda.Id = 0; 
            agenda.GrupoId = id;
            grupoDb.Agendas.Add(agenda);
        }

        // Atualiza Usuários (Sincronização)
        grupoDb.Usuarios.Clear();
        if (grupoInput.Usuarios != null)
        {
            foreach (var u in grupoInput.Usuarios)
            {
                // Busca o usuário gerenciado pelo contexto para evitar duplicação ou erro de tracking
                var usuarioDb = await _usuarioRepository.ProcurarUsuarioById(u.Id);
                if (usuarioDb != null)
                {
                    grupoDb.Usuarios.Add(usuarioDb);
                }
            }
        }
        
        await _grupoRepository.UpdateGrupo(grupoDb);
    }

    public async Task Deletar(int id)
    {
        var grupo = await _grupoRepository.GetGroupById(id);
        if (grupo != null)
        {
            // Graças à configuração no AppDbContext, isso deletará as Agendas em cascata
            await _grupoRepository.DeletarGrupo(grupo);
        }
    }

    // 1. Criador adiciona usuários e o limite sobe se necessário
    public async Task<String> AdicionarUsuario(int grupoId, int usuarioId, int solicitanteId)
    {
        var grupo = await _grupoRepository.GetGroupById(grupoId);
        if (grupo == null) throw new Exception("Grupo não encontrado.");

        if (grupo.CriadorId != solicitanteId)
            throw new UnauthorizedAccessException("Apenas o criador pode adicionar membros.");

        if (grupo.Usuarios.Count >= grupo.CapacidadeMaxima)
            grupo.CapacidadeMaxima++;

        var usuario = await _usuarioRepository.ProcurarUsuarioById(usuarioId);

        if (usuario != null)
        {
            grupo.Usuarios.Add(usuario);

            await _grupoRepository.UpdateGrupo(grupo);

            return "Usuário adicionado e limite atualizado.";
        }

        return "Erro ao adicionar usuário";
    }

    // 2. Visualização Mensal
    public async Task<Dictionary<string, List<ResumoRefeicaoDTO>>> ObterAgendaMensal(int? mes, int? ano)
    {
        int consultaMes = mes ?? DateTime.Now.Month;
        int consultaAno = ano ?? DateTime.Now.Year;

        var inicio = new DateTime(consultaAno, consultaMes, 1);
        var fim = inicio.AddMonths(2).AddDays(-1); // Mês atual e próximo

        var grupos = await _grupoRepository.GetGruposWithUsuariosAndAgendas();

        var resultado = new List<ResumoRefeicaoDTO>();

        for (var dia = inicio; dia <= fim; dia = dia.AddDays(1))
        {
            foreach (var g in grupos)
            {
                var temAgenda = g.Agendas.Any(a =>
                    (a.EhRecorrente && a.DiaSemana == dia.DayOfWeek) ||
                    (!a.EhRecorrente && a.DataEspecifica?.Date == dia.Date));

                if (temAgenda)
                {
                    foreach (var agenda in g.Agendas.Where(a => (a.EhRecorrente && a.DiaSemana == dia.DayOfWeek) || (!a.EhRecorrente && a.DataEspecifica?.Date == dia.Date)))
                    {
                        resultado.Add(new ResumoRefeicaoDTO
                        {
                            Data = dia.ToString("dd/MM/yyyy"),
                            Refeicao = agenda.Refeicao.ToString(),
                            Grupo = g.Nome,
                            QuantidadePessoas = g.Usuarios.Count,
                            Limite = g.CapacidadeMaxima
                        });
                    }
                }
            }
        }

        // Agrupa por Mês/Ano para o Front-end
        return resultado.GroupBy(r => DateTime.Parse(r.Data!).ToString("MMMM yyyy"))
                .ToDictionary(g => g.Key, g => g.ToList());
    }
}
