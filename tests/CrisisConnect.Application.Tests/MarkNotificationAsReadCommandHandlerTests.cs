using CrisisConnect.Application.UseCases.Notifications.MarkAsRead;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class MarkNotificationAsReadCommandHandlerTests
{
    private readonly INotificationRepository _notifRepo = Substitute.For<INotificationRepository>();

    private MarkNotificationAsReadCommandHandler CréerHandler() => new(_notifRepo);

    [Fact]
    public async Task MarkAsRead_NotificationExiste_MarquéeLueEtPersistée()
    {
        // Arrange
        var notification = new Notification(Guid.NewGuid(), TypeNotification.IntentionDonAcceptee, "Test");
        _notifRepo.GetByIdAsync(notification.Id, Arg.Any<CancellationToken>())
            .Returns(notification);

        // Act
        await CréerHandler().Handle(new MarkNotificationAsReadCommand(notification.Id), CancellationToken.None);

        // Assert
        Assert.True(notification.EstLue);
        await _notifRepo.Received(1).UpdateAsync(notification, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task MarkAsRead_NotificationIntrouvable_LèveNotFoundException()
    {
        // Arrange
        var notifId = Guid.NewGuid();
        _notifRepo.GetByIdAsync(notifId, Arg.Any<CancellationToken>())
            .Returns((Notification?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new MarkNotificationAsReadCommand(notifId), CancellationToken.None).AsTask());
    }
}
