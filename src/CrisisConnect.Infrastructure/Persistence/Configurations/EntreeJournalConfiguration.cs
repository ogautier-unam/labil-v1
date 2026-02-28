using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class EntreeJournalConfiguration : IEntityTypeConfiguration<EntreeJournal>
{
    public void Configure(EntityTypeBuilder<EntreeJournal> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.TypeOperation).HasConversion<string>().IsRequired();
        builder.Property(e => e.EntiteCibleType).HasMaxLength(100);
        builder.Property(e => e.Details).HasMaxLength(2000);
        builder.Property(e => e.AdresseIP).HasMaxLength(45);
        builder.Property(e => e.SessionId).HasMaxLength(100);
        builder.ToTable("entrees_journal");
    }
}
