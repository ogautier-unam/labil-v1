using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class EntiteConfiguration : IEntityTypeConfiguration<Entite>
{
    public void Configure(EntityTypeBuilder<Entite> builder)
    {
        builder.Property(e => e.Nom).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(2000);
        builder.Property(e => e.MoyensContact).HasMaxLength(500);
        builder.Property(e => e.UrlPagePresentation).HasMaxLength(500);
        builder.Property(e => e.CommentFaireDon).HasMaxLength(1000);
        builder.Property(e => e.TypesContributions).HasMaxLength(500);
    }
}
