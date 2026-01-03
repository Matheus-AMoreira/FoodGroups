using System.ComponentModel.DataAnnotations;

namespace FoodGroups.DTOs;

public class ResumoRefeicaoDTO
{
    [Required]
    public string? Data { get; set; }

    [Required]
    public string? Refeicao { get; set; }

    [Required]
    public string? Grupo { get; set; }

    public int QuantidadePessoas { get; set; }
    public int Limite { get; set; }
}
