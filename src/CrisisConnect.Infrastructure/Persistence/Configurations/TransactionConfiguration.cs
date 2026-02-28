using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Statut).HasConversion<string>().IsRequired();

        builder.HasOne(t => t.Discussion)
            .WithOne()
            .HasForeignKey<Discussion>(d => d.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("transactions");
    }
}
