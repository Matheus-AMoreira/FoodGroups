using System.ComponentModel.DataAnnotations;

namespace FoodGroups.DTOs;

public class ResumoRefeicaoDTO
{
    public string? Data { get; set; }
    public string? Tipo { get; set; }
    public string? Descricao { get; set; }
    public int QuantidadePessoas { get; set; }
    public int Limite { get; set; }
}