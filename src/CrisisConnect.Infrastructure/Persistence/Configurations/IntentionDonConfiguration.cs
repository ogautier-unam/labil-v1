using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class IntentionDonConfiguration : IEntityTypeConfiguration<IntentionDon>
{
    public void Configure(EntityTypeBuilder<IntentionDon> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Unite).IsRequired().HasMaxLength(50);
        builder.Property(i => i.Description).HasMaxLength(500);
        builder.Property(i => i.Statut).HasConversion<string>().IsRequired();

        builder.ToTable("intentions_don");
    }
}
