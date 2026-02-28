using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class MatchingConfiguration : IEntityTypeConfiguration<Matching>
{
    public void Configure(EntityTypeBuilder<Matching> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Statut).HasConversion<string>().IsRequired();
        builder.ToTable("matchings");
    }
}
