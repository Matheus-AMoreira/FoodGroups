using FoodGroups.Models;
using FoodGroups.DTOs;
using FoodGroups.Interfaces;
using FoodGroups.Validators;

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
        if (grupo.CriadorId <= 0) return "O grupo precisa ter um dono.";

        // Validação de Conflito no Back (Segurança final)
        var agendasValidadas = new List<AgendaGrupo>();
        foreach (var agenda in grupo.Agendas)
        {
            var erro = AgendaValidator.VerificarConflito(agendasValidadas, agenda);
            if (erro != null) return $"Erro de validação: {erro}";
            agendasValidadas.Add(agenda);
        }

        var criador = await _usuarioRepository.ProcurarUsuarioById(grupo.CriadorId);
        if (criador == null) return "Usuário dono não encontrado.";

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
        int m = mes ?? DateTime.Now.Month;
        int a = ano ?? DateTime.Now.Year;
        
        var dataInicio = new DateTime(a, m, 1);
        var dataFim = dataInicio.AddMonths(1).AddDays(-1);

        var grupos = await _grupoRepository.GetGruposWithUsuariosAndAgendas();
        var resultado = new List<ResumoRefeicaoDTO>();

        foreach (var grupo in grupos)
        {
            foreach (var agenda in grupo.Agendas)
            {
                // Lógica de Desempenho: Projetar apenas datas dentro do mês solicitado
                // Limite de recorrência: 1 ano a partir de hoje (Regra de Negócio)
                var limiteRecorrencia = DateTime.Now.AddYears(1);

                if (agenda.EhRecorrente && agenda.DiaSemana.HasValue)
                {
                    // Projeta os dias da semana dentro do mês alvo
                    for (var d = dataInicio; d <= dataFim; d = d.AddDays(1))
                    {
                        if (d > limiteRecorrencia) break;

                        if (d.DayOfWeek == agenda.DiaSemana.Value)
                        {
                            resultado.Add(MontarDTO(d, agenda, grupo));
                        }
                    }
                }
                else if (!agenda.EhRecorrente && agenda.DataEspecifica.HasValue)
                {
                    var data = agenda.DataEspecifica.Value;
                    if (data >= dataInicio && data <= dataFim)
                    {
                        resultado.Add(MontarDTO(data, agenda, grupo));
                    }
                }
            }
        }

        return resultado
            .OrderBy(x => DateTime.Parse(x.Data!))
            .GroupBy(r => r.Data!)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    private ResumoRefeicaoDTO MontarDTO(DateTime data, AgendaGrupo agenda, Grupo grupo)
    {
        return new ResumoRefeicaoDTO
        {
            Data = data.ToString("yyyy-MM-dd"), // Formato ISO para facilitar ordenação no front
            Refeicao = agenda.Refeicao.ToString(),
            Grupo = grupo.Nome,
            QuantidadePessoas = grupo.Usuarios.Count,
            Limite = grupo.CapacidadeMaxima
        };
    }
}
