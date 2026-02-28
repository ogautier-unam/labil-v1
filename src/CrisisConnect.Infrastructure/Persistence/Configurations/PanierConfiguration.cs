using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class PanierConfiguration : IEntityTypeConfiguration<Panier>
{
    public void Configure(EntityTypeBuilder<Panier> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Statut).HasConversion<string>().IsRequired();

        builder.HasMany(p => p.Offres)
            .WithMany()
            .UsingEntity("paniers_offres");

        builder.ToTable("paniers");
    }
}
