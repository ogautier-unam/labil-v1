using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class DemandeRepartitionGeoConfiguration : IEntityTypeConfiguration<DemandeRepartitionGeo>
{
    public void Configure(EntityTypeBuilder<DemandeRepartitionGeo> builder)
    {
        builder.Property(d => d.DescriptionMission).IsRequired().HasMaxLength(1000);
    }
}
