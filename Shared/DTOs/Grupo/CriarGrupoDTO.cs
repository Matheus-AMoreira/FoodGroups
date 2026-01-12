using System.ComponentModel.DataAnnotations;
using FoodGroups.Shared.Models;

namespace FoodGroups.Shared.DTOs;

public class CriarGrupoDTO
{
    [Required]
    public required string Nome { get; set; }

    [Required]
    public int CapacidadeMaxima { get; set; }

    [Required]
    public int CriadorId { get; set; }

    public List<Usuario>? Usuarios { get; set; }

    public List<AgendaGrupo> Agendas { get; set; } = new();
}
