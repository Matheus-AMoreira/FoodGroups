using System.ComponentModel.DataAnnotations;

namespace FoodGroups.Models;

public class Grupo
{
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public int CapacidadeMaxima { get; set; }

    [Required]
    public int CriadorId { get; set; }

    public List<Usuario> Usuarios { get; set; } = new();

    public List<AgendaGrupo> Agendas { get; set; } = new();
}
