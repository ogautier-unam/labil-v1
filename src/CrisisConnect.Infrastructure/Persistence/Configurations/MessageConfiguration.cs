using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Contenu).IsRequired().HasMaxLength(4000);
        builder.Property(m => m.Langue).HasMaxLength(10);
        builder.Property(m => m.TexteOriginal).HasMaxLength(4000);
        builder.ToTable("messages");
    }
}
