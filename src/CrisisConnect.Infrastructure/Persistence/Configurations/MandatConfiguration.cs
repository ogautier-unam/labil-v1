using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class MandatConfiguration : IEntityTypeConfiguration<Mandat>
{
    public void Configure(EntityTypeBuilder<Mandat> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Portee).HasConversion<string>().IsRequired();
        builder.Property(m => m.Description).HasMaxLength(1000);
        builder.ToTable("mandats");
    }
}
