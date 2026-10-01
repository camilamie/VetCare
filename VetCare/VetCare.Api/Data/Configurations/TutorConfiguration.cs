using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetCare.Api.Models;

namespace VetCare.Api.Data.Configurations;

public class TutorConfiguration : IEntityTypeConfiguration<Tutor>
{
    public void Configure(EntityTypeBuilder<Tutor> builder)
    {
        builder.ToTable("Tutores");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Cpf)
            .IsRequired()
            .HasMaxLength(11);

        builder.HasIndex(t => t.Cpf)
            .IsUnique();

        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(t => t.Telefone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(t => t.DataCadastro)
            .IsRequired();

        builder.HasMany(t => t.Pets)
            .WithOne(p => p.Tutor)
            .HasForeignKey(p => p.TutorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
