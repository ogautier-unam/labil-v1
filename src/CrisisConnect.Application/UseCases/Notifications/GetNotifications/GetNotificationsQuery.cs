using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Notifications.GetNotifications;

public record GetNotificationsQuery(Guid DestinataireId) : IRequest<IReadOnlyList<NotificationDto>>;
