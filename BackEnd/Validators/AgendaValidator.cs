using FoodGroups.Models;

namespace FoodGroups.Validators;

public static class AgendaValidator
{
    public static string? VerificarConflito(List<AgendaGrupo> agendasExistentes, AgendaGrupo novaAgenda)
    {
        foreach (var agenda in agendasExistentes)
        {
            // Regra: Mesma refeição (Ex: não pode ter dois Cafés no mesmo dia para o mesmo grupo)
            if (agenda.Refeicao != novaAgenda.Refeicao) continue;

            // 1. Ambas são datas específicas
            if (!agenda.EhRecorrente && !novaAgenda.EhRecorrente)
            {
                if (agenda.DataEspecifica?.Date == novaAgenda.DataEspecifica?.Date)
                    return $"Já existe um {agenda.Refeicao} agendado para o dia {agenda.DataEspecifica:dd/MM/yyyy}.";
            }
            // 2. Ambas são recorrentes (Ex: Toda Segunda)
            else if (agenda.EhRecorrente && novaAgenda.EhRecorrente)
            {
                if (agenda.DiaSemana == novaAgenda.DiaSemana)
                    return $"Já existe um {agenda.Refeicao} recorrente para {agenda.DiaSemana}.";
            }
            // 3. Mistas (Uma específica e uma recorrente)
            else
            {
                var recorrente = agenda.EhRecorrente ? agenda : novaAgenda;
                var especifica = agenda.EhRecorrente ? novaAgenda : agenda;

                if (especifica.DataEspecifica?.DayOfWeek == recorrente.DiaSemana)
                {
                    return $"Conflito: O {especifica.Refeicao} do dia {especifica.DataEspecifica:dd/MM/yyyy} coincide com a recorrência de {recorrente.DiaSemana}.";
                }
            }
        }
        return null; // Sem conflitos
    }
}