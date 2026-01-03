namespace FoodGroups.Models;

public class Grupo
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int CapacidadeMaxima { get; set; }
    public int CriadorId { get; set; }
    public List<Usuario> Usuarios { get; set; } = new();
    public List<AgendaGrupo> Agendas { get; set; } = new();
}
