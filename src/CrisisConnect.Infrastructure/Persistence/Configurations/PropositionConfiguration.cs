using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class PropositionConfiguration : IEntityTypeConfiguration<Proposition>
{
    public void Configure(EntityTypeBuilder<Proposition> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasDiscriminator<string>("type_proposition")
            .HasValue<Offre>("Offre")
            .HasValue<Demande>("Demande");

        builder.Property(p => p.Titre)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(p => p.Statut)
            .HasConversion<string>()
            .IsRequired();

        builder.OwnsOne(p => p.Localisation, l =>
        {
            l.Property(x => x.Latitude).HasColumnName("latitude");
            l.Property(x => x.Longitude).HasColumnName("longitude");
        });

        builder.ToTable("propositions");
    }
}
