using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class MissionConfiguration : IEntityTypeConfiguration<Mission>
{
    public void Configure(EntityTypeBuilder<Mission> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Titre).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Description).IsRequired().HasMaxLength(2000);
        builder.Property(m => m.Statut).HasConversion<string>().IsRequired();
        builder.Property(m => m.NombreBenevoles).IsRequired();

        builder.OwnsOne(m => m.Plage, p =>
        {
            p.Property(x => x.Debut).HasColumnName("plage_debut");
            p.Property(x => x.Fin).HasColumnName("plage_fin");
        });

        builder.HasMany(m => m.Matchings)
            .WithOne()
            .HasForeignKey(x => x.MissionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("missions");
    }
}
