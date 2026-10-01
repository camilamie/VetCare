using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetCare.Api.Models;

namespace VetCare.Api.Data.Configurations;

public class ConsultaConfiguration : IEntityTypeConfiguration<Consulta>
{
    public void Configure(EntityTypeBuilder<Consulta> builder)
    {
        builder.ToTable("Consultas");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.DataHora)
            .IsRequired();

        builder.Property(c => c.Veterinario)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Motivo)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(c => c.Diagnostico)
            .HasMaxLength(500);

        builder.Property(c => c.Valor)
            .HasPrecision(10, 2);

        // Enum salvo como texto ("Agendada", "Realizada", "Cancelada") para facilitar a leitura no banco
        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}
