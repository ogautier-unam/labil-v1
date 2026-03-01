using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Notifications.GetNotifications;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetNotificationsQueryHandlerTests
{
    private readonly INotificationRepository _notifRepo = Substitute.For<INotificationRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetNotificationsQueryHandler CréerHandler() => new(_notifRepo, _mapper);

    [Fact]
    public async Task GetNotifications_DeuxNotifications_RetourneDeuxDtos()
    {
        // Arrange
        var destinataireId = Guid.NewGuid();
        var notifications = new List<Notification>
        {
            new(destinataireId, TypeNotification.IntentionDonAcceptee, "Votre don a été accepté"),
            new(destinataireId, TypeNotification.SuggestionAppariementDisponible, "Un appariement est disponible")
        };
        _notifRepo.GetByDestinataireAsync(destinataireId, Arg.Any<CancellationToken>())
            .Returns(notifications.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetNotificationsQuery(destinataireId), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, n => n.Type == TypeNotification.IntentionDonAcceptee);
        Assert.Contains(result, n => n.Type == TypeNotification.SuggestionAppariementDisponible);
    }

    [Fact]
    public async Task GetNotifications_AucuneNotification_RetourneListeVide()
    {
        // Arrange
        var destinataireId = Guid.NewGuid();
        _notifRepo.GetByDestinataireAsync(destinataireId, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Notification>());

        // Act
        var result = await CréerHandler().Handle(new GetNotificationsQuery(destinataireId), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
