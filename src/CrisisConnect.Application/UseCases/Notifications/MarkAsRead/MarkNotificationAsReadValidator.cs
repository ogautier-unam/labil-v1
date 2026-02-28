using FluentValidation;

namespace CrisisConnect.Application.UseCases.Notifications.MarkAsRead;

public class MarkNotificationAsReadValidator : AbstractValidator<MarkNotificationAsReadCommand>
{
    public MarkNotificationAsReadValidator()
    {
        RuleFor(x => x.NotificationId).NotEmpty();
    }
}
