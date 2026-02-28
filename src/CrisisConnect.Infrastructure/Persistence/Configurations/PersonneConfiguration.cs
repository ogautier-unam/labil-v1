using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class PersonneConfiguration : IEntityTypeConfiguration<Personne>
{
    public void Configure(EntityTypeBuilder<Personne> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.Prenom)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Nom)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Telephone)
            .HasMaxLength(20);

        builder.OwnsOne(p => p.Adresse, a =>
        {
            a.Property(x => x.Rue).HasColumnName("adresse_rue").HasMaxLength(200);
            a.Property(x => x.Ville).HasColumnName("adresse_ville").HasMaxLength(100);
            a.Property(x => x.CodePostal).HasColumnName("adresse_code_postal").HasMaxLength(10);
            a.Property(x => x.Pays).HasColumnName("adresse_pays").HasMaxLength(100);
        });

        builder.ToTable("personnes");
    }
}
