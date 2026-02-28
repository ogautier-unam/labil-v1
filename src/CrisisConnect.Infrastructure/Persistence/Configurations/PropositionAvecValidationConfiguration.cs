using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class PropositionAvecValidationConfiguration : IEntityTypeConfiguration<PropositionAvecValidation>
{
    public void Configure(EntityTypeBuilder<PropositionAvecValidation> builder)
    {
        builder.Property(p => p.DescriptionValidation).HasMaxLength(1000);
        builder.Property(p => p.StatutValidation).HasConversion<string>().IsRequired();
    }
}
