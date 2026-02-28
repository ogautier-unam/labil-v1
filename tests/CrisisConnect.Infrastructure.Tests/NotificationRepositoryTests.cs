using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class NotificationRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetByIdAsync_RetourneNotification()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new NotificationRepository(ctx);

        var destinataireId = Guid.NewGuid();
        var notification = new Notification(destinataireId, TypeNotification.IntentionDonAcceptee, "Votre don a été accepté");
        await repo.AddAsync(notification);

        var résultat = await repo.GetByIdAsync(notification.Id);

        Assert.NotNull(résultat);
        Assert.Equal(destinataireId, résultat.DestinataireId);
        Assert.Equal(TypeNotification.IntentionDonAcceptee, résultat.Type);
        Assert.False(résultat.EstLue);
    }

    [Fact]
    public async Task GetByDestinataireAsync_RetourneSeulementLesNotificationsDuDestinataire()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new NotificationRepository(ctx);

        var dest1 = Guid.NewGuid();
        var dest2 = Guid.NewGuid();
        await repo.AddAsync(new Notification(dest1, TypeNotification.IntentionDonAcceptee, "Notif 1"));
        await repo.AddAsync(new Notification(dest1, TypeNotification.SuggestionAppariementDisponible, "Notif 2"));
        await repo.AddAsync(new Notification(dest2, TypeNotification.IntentionDonRefusee, "Notif autre"));

        var résultats = await repo.GetByDestinataireAsync(dest1);

        Assert.Equal(2, résultats.Count);
        Assert.All(résultats, n => Assert.Equal(dest1, n.DestinataireId));
    }

    [Fact]
    public async Task GetByIdAsync_IdInexistant_RetourneNull()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new NotificationRepository(ctx);

        var résultat = await repo.GetByIdAsync(Guid.NewGuid());

        Assert.Null(résultat);
    }

    [Fact]
    public async Task UpdateAsync_MarquerCommeLue_EstLuePersistée()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new NotificationRepository(ctx);

        var notification = new Notification(Guid.NewGuid(), TypeNotification.PanierAnnuleOffreRemiseActive, "Offre remise active");
        await repo.AddAsync(notification);

        notification.MarquerCommeLue();
        await repo.UpdateAsync(notification);

        var chargée = await repo.GetByIdAsync(notification.Id);
        Assert.True(chargée!.EstLue);
    }
}
