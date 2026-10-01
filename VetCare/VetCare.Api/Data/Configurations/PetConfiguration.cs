using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetCare.Api.Models;

namespace VetCare.Api.Data.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("Pets");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(p => p.Especie)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(p => p.Raca)
            .HasMaxLength(60);

        builder.Property(p => p.PesoKg)
            .HasPrecision(6, 2);

        builder.HasMany(p => p.Consultas)
            .WithOne(c => c.Pet)
            .HasForeignKey(c => c.PetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
