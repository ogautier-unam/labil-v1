using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class MethodeIdentificationConfiguration : IEntityTypeConfiguration<MethodeIdentification>
{
    public void Configure(EntityTypeBuilder<MethodeIdentification> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasDiscriminator<string>("type_methode")
            .HasValue<LoginPassword>("LoginPassword")
            .HasValue<CarteIdentiteElectronique>("CarteIdentiteElectronique")
            .HasValue<VerificationSMS>("VerificationSMS")
            .HasValue<VerificationBancaire>("VerificationBancaire")
            .HasValue<VerificationFacture>("VerificationFacture")
            .HasValue<VerificationPhoto>("VerificationPhoto")
            .HasValue<Parrainage>("Parrainage")
            .HasValue<Delegation>("Delegation");

        builder.Property(m => m.NiveauFiabilite).HasConversion<string>().IsRequired();

        builder.HasOne<Personne>()
            .WithMany()
            .HasForeignKey(m => m.PersonneId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("methodes_identification");
    }
}

public class ParrainageConfiguration : IEntityTypeConfiguration<Parrainage>
{
    public void Configure(EntityTypeBuilder<Parrainage> builder)
    {
        var converter = new ValueConverter<List<Guid>, string>(
            v => string.Join(',', v),
            v => string.IsNullOrEmpty(v)
                ? new List<Guid>()
                : v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());

        var comparer = new ValueComparer<List<Guid>>(
            (a, b) => a!.SequenceEqual(b!),
            c => c.Aggregate(0, (h, g) => HashCode.Combine(h, g.GetHashCode())),
            c => c.ToList());

        builder.Property<List<Guid>>("_parrainsIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("parrains_ids")
            .HasConversion(converter, comparer)
            .HasColumnType("text")
            .IsRequired(false);
    }
}
