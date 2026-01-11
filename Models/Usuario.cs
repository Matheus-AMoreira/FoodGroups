using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FoodGroups.Models;

[Index(nameof(Nome), nameof(Email), IsUnique = true)]
public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Senha { get; set; } = string.Empty;

    public List<Grupo> Grupo { get; set; } = new();
}
