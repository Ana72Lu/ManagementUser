using Microsoft.EntityFrameworkCore;
using ManagementUser.Models;

namespace ManagementUser.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Perfil> Perfis { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração do relacionamento
        modelBuilder.Entity<User>()
            .HasOne(u => u.Perfil) // Um usuário tem um perfil
            .WithMany(p => p.Usuarios) // Um perfil tem muitos usuários
            .HasForeignKey(u => u.PerfilId) // Chave estrangeira
            .OnDelete(DeleteBehavior.Restrict); // Evita deletar perfis com usuários vinculados

        // Seed inicial de perfis
        modelBuilder.Entity<Perfil>().HasData(
            new Perfil { Id = 1, Nome = "Admin", Descricao = "Administrador do sistema" },
            new Perfil { Id = 2, Nome = "Cliente", Descricao = "Usuário comum" },
            new Perfil { Id = 3, Nome = "Gerente", Descricao = "Gerente de operações" }
        );
    }
}