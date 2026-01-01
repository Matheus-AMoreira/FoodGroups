using FoodGroups.model;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Tabelas
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<AgendaGrupo> AgendaGrupos { get; set; }
}