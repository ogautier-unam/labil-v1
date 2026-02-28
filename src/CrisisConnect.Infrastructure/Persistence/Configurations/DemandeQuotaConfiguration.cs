using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class DemandeQuotaConfiguration : IEntityTypeConfiguration<DemandeQuota>
{
    public void Configure(EntityTypeBuilder<DemandeQuota> builder)
    {
        builder.Property(d => d.UniteCapacite).IsRequired().HasMaxLength(50);
        builder.Property(d => d.AdresseDepot).HasMaxLength(300);

        builder.HasMany(d => d.Intentions)
            .WithOne()
            .HasForeignKey(i => i.DemandeQuotaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
