using FoodGroups.Shared.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        ArgumentNullException.ThrowIfNull(options);
    }

    // Tabelas
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<AgendaGrupo> AgendaGrupos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Delete em cascada
        modelBuilder.Entity<Grupo>()
            .HasMany(g => g.Agendas)
            .WithOne()
            .HasForeignKey(a => a.GrupoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice Único para Grupo.Nome
        modelBuilder.Entity<Grupo>()
            .HasIndex(g => g.Nome)
            .IsUnique();

        // Índice Único para Usuario
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => new { u.Nome, u.Email })
            .IsUnique();
    }
}
