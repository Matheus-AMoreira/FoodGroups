using FoodGroups.Models;
using FoodGroups.DTOs;

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
        if (grupo.Agendas == null)
        {
            return "sem sgenda";
        }
        if (grupo.CapacidadeMaxima >= 0)
        {
            return "Capacidade precisa ser maior que 1";
        }

        await _grupoRepository.CriarGrupo(grupo);

        return "oi";
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
        return resultado.GroupBy(r => DateTime.Parse(r.Data).ToString("MMMM yyyy"))
                        .ToDictionary(g => g.Key, g => g.ToList());
    }
}
