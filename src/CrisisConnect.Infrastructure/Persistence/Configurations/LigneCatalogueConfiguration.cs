using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class LigneCatalogueConfiguration : IEntityTypeConfiguration<LigneCatalogue>
{
    public void Configure(EntityTypeBuilder<LigneCatalogue> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Reference).IsRequired().HasMaxLength(100);
        builder.Property(l => l.Designation).IsRequired().HasMaxLength(300);
        builder.Property(l => l.UrlProduit).HasMaxLength(500);
        builder.Property(l => l.Statut).HasConversion<string>().IsRequired();

        builder.ToTable("lignes_catalogue");
    }
}
