using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class SuggestionAppariementConfiguration : IEntityTypeConfiguration<SuggestionAppariement>
{
    public void Configure(EntityTypeBuilder<SuggestionAppariement> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Raisonnement).HasMaxLength(2000);

        builder.HasOne<Offre>()
            .WithMany()
            .HasForeignKey(s => s.OffreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Demande>()
            .WithMany()
            .HasForeignKey(s => s.DemandeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("suggestions_appariement");
    }
}
