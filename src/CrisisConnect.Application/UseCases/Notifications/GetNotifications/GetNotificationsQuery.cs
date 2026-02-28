using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Notifications.GetNotifications;

public record GetNotificationsQuery(Guid DestinataireId) : IRequest<IReadOnlyList<NotificationDto>>;
