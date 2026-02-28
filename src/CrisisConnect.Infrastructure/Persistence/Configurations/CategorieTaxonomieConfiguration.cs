using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class CategorieTaxonomieConfiguration : IEntityTypeConfiguration<CategorieTaxonomie>
{
    public void Configure(EntityTypeBuilder<CategorieTaxonomie> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code).IsRequired().HasMaxLength(100);
        builder.Property(c => c.NomJson).IsRequired().HasColumnType("jsonb");
        builder.Property(c => c.DescriptionJson).IsRequired().HasColumnType("jsonb");

        builder.HasIndex(c => new { c.Code, c.ConfigId }).IsUnique();

        // Composite pattern : auto-référence parent/sous-catégories
        builder.HasMany(c => c.SousCategories)
            .WithOne()
            .HasForeignKey(c => c.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ConfigCatastrophe>()
            .WithMany()
            .HasForeignKey(c => c.ConfigId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("categories_taxonomie");
    }
}
