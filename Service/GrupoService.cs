
using FoodGroups.DTO;
using Microsoft.EntityFrameworkCore;

namespace FoodGroups.Service;

public class GrupoService
{
    private readonly AppDbContext _context;

    public GrupoService(AppDbContext context) => _context = context;

    // 1. Criador adiciona usuários e o limite sobe se necessário
    public async Task AdicionarUsuario(int grupoId, int usuarioId, int solicitanteId)
    {
        var grupo = await _context.Grupos.Include(g => g.Usuarios).FirstOrDefaultAsync(g => g.Id == grupoId);
        if (grupo == null) throw new Exception("Grupo não encontrado.");

        if (grupo.CriadorId != solicitanteId)
            throw new UnauthorizedAccessException("Apenas o criador pode adicionar membros.");

        // Se encher, aumenta o limite automaticamente
        if (grupo.Usuarios.Count >= grupo.CapacidadeMaxima)
            grupo.CapacidadeMaxima++;

        var usuario = await _context.Usuarios.FindAsync(usuarioId);
        grupo.Usuarios.Add(usuario!);
        await _context.SaveChangesAsync();
    }

    // 2. Visualização Mensal
    public async Task<Dictionary<string, List<ResumoRefeicaoDTO>>> ObterAgendaMensal(int? mes, int? ano)
    {
        int consultaMes = mes ?? DateTime.Now.Month;
        int consultaAno = ano ?? DateTime.Now.Year;

        var inicio = new DateTime(consultaAno, consultaMes, 1);
        var fim = inicio.AddMonths(2).AddDays(-1); // Mês atual e próximo

        var grupos = await _context.Grupos
            .Include(g => g.Usuarios)
            .Include(g => g.Agendas)
            .ToListAsync();

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
