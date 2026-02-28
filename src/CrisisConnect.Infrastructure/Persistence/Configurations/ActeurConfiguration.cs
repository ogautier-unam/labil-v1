using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class ActeurConfiguration : IEntityTypeConfiguration<Acteur>
{
    public void Configure(EntityTypeBuilder<Acteur> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasDiscriminator<string>("type_acteur")
            .HasValue<Personne>("Personne")
            .HasValue<Entite>("Entite");

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(a => a.Email).IsUnique();

        builder.Property(a => a.Role).IsRequired().HasMaxLength(50);

        builder.ToTable("acteurs");
    }
}
