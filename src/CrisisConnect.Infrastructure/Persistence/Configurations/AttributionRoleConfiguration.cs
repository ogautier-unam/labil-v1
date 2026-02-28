using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class AttributionRoleConfiguration : IEntityTypeConfiguration<AttributionRole>
{
    public void Configure(EntityTypeBuilder<AttributionRole> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.TypeRole).HasConversion<string>().IsRequired();
        builder.Property(a => a.Statut).HasConversion<string>().IsRequired();
        builder.ToTable("attributions_roles");
    }
}
