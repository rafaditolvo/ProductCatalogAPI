using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Ignore todas as advertências sensíveis relacionadas a dados
        optionsBuilder
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.SensitiveDataLoggingEnabledWarning));

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações adicionais, se necessário

        // Configurar chave primária
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);

        // Exemplo: Configurar nome da tabela
        modelBuilder.Entity<Product>()
            .ToTable("Products");
    }
}
