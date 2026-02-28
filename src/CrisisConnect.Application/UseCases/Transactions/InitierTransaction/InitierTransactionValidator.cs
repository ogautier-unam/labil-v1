using FluentValidation;

namespace CrisisConnect.Application.UseCases.Transactions.InitierTransaction;

public class InitierTransactionValidator : AbstractValidator<InitierTransactionCommand>
{
    public InitierTransactionValidator()
    {
        RuleFor(x => x.PropositionId).NotEmpty();
        RuleFor(x => x.InitiateurId).NotEmpty();
    }
}
