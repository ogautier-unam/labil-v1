using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class OffreConfiguration : IEntityTypeConfiguration<Offre>
{
    public void Configure(EntityTypeBuilder<Offre> builder)
    {
        builder.Property(o => o.LivraisonIncluse).HasDefaultValue(false);

        builder.Navigation(o => o.DemandesCouplees)
            .HasField("_demandesCouplees")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(o => o.DemandesCouplees)
            .WithMany()
            .UsingEntity("offre_demandes_couplees");
    }
}
