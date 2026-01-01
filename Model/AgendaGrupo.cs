namespace FoodGroups.model;

public enum TipoRefeicao { Cafe, Almoco, Jantar }

public class AgendaGrupo
{
    public int Id { get; set; }
    public int GrupoId { get; set; }
    public TipoRefeicao Refeicao { get; set; }
    public DayOfWeek? DiaSemana { get; set; }
    public DateTime? DataEspecifica { get; set; }
    public bool EhRecorrente { get; set; }
}
