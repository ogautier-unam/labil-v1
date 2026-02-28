using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrisisConnect.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Type).HasConversion<string>().IsRequired();
        builder.Property(n => n.Contenu).IsRequired().HasMaxLength(2000);
        builder.Property(n => n.RefEntiteId).HasMaxLength(200);
        builder.ToTable("notifications");
    }
}
