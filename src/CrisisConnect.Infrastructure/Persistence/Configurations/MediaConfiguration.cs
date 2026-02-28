using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Url).IsRequired().HasMaxLength(500);
        builder.Property(m => m.Type).HasConversion<string>().IsRequired();

        builder.HasOne<Proposition>()
            .WithMany(p => p.Medias)
            .HasForeignKey(m => m.PropositionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("medias");
    }
}
