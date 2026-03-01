using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Notifications.MarkAsRead;

public class MarkNotificationAsReadCommandHandler : ICommandHandler<MarkNotificationAsReadCommand>
{
    private readonly INotificationRepository _repository;

    public MarkNotificationAsReadCommandHandler(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<Unit> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _repository.GetByIdAsync(request.NotificationId, cancellationToken)
            ?? throw new NotFoundException(nameof(Notification), request.NotificationId);

        notification.MarquerCommeLue();
        await _repository.UpdateAsync(notification, cancellationToken);
        return Unit.Value;
    }
}
