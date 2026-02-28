using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class DemandeSurCatalogueConfiguration : IEntityTypeConfiguration<DemandeSurCatalogue>
{
    public void Configure(EntityTypeBuilder<DemandeSurCatalogue> builder)
    {
        builder.Property(d => d.UrlCatalogue).IsRequired().HasMaxLength(500);

        builder.HasMany(d => d.Lignes)
            .WithOne()
            .HasForeignKey(l => l.DemandeSurCatalogueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
