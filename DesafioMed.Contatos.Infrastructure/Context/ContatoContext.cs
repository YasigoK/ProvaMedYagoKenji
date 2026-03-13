using DesafioMed.Contatos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioMed.Contatos.Infrastructure.Context;

public class ContatoContext : DbContext
{
    public ContatoContext(DbContextOptions<ContatoContext> options) : base(options) { }


    public DbSet<ContatoEntity> ContatoProva { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContatoContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
