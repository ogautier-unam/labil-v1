using Mediator;

namespace CrisisConnect.Application.UseCases.Notifications.MarkAsRead;

public record MarkNotificationAsReadCommand(Guid NotificationId) : ICommand;
