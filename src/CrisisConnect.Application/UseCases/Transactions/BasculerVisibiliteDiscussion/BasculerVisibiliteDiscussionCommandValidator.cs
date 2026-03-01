using FluentValidation;

namespace CrisisConnect.Application.UseCases.Transactions.BasculerVisibiliteDiscussion;

public class BasculerVisibiliteDiscussionCommandValidator : AbstractValidator<BasculerVisibiliteDiscussionCommand>
{
    public BasculerVisibiliteDiscussionCommandValidator()
    {
        RuleFor(x => x.TransactionId).NotEmpty();
    }
}
