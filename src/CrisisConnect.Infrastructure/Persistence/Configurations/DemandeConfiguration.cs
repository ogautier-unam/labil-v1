using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class DemandeConfiguration : IEntityTypeConfiguration<Demande>
{
    public void Configure(EntityTypeBuilder<Demande> builder)
    {
        builder.Property(d => d.OperateurLogique).HasConversion<string>().IsRequired();
        builder.Property(d => d.Urgence).HasConversion<string>().IsRequired();
        builder.Property(d => d.RegionSeverite).HasMaxLength(200);

        // Composite pattern : auto-référence parent/sous-demandes
        builder.HasMany(d => d.SousDemandes)
            .WithOne()
            .HasForeignKey(d => d.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
