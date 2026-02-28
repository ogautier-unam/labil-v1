using MediatR;

namespace CrisisConnect.Application.UseCases.Notifications.MarkAsRead;

public record MarkNotificationAsReadCommand(Guid NotificationId) : IRequest;
