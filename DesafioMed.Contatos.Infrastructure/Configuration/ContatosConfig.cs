using DesafioMed.Contatos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioMed.Contatos.Infrastructure.Configuration;

public class ContatosConfig : IEntityTypeConfiguration<ContatoEntity>
{
    public void Configure(EntityTypeBuilder<ContatoEntity> builder)
    {
        builder.ToTable("ContatoProva");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .HasColumnName("Nome")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.DataNascimento)
            .HasColumnName("DataDeNascimento")
            .HasColumnType("DATE")
            .IsRequired();

        builder.Property(x => x.Sexo)
            .HasColumnName("Sexo")
            .IsRequired();

        builder.Property(x => x.Ativo)
            .HasColumnName("Ativo");

        builder.Ignore(x => x.Idade);


    }

}
