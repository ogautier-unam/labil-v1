using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class ConfigCatastropheConfiguration : IEntityTypeConfiguration<ConfigCatastrophe>
{
    public void Configure(EntityTypeBuilder<ConfigCatastrophe> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nom).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Description).HasMaxLength(2000);
        builder.Property(c => c.ZoneGeographique).IsRequired().HasMaxLength(300);
        builder.Property(c => c.EtatReferent).IsRequired().HasMaxLength(200);
        builder.Property(c => c.LanguesActives).IsRequired().HasMaxLength(500);
        builder.Property(c => c.ModesIdentificationActifs).IsRequired().HasMaxLength(1000);

        builder.ToTable("config_catastrophes");
    }
}
