namespace FoodGroups.model;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public List<Grupo> Grupo { get; set; }
}
