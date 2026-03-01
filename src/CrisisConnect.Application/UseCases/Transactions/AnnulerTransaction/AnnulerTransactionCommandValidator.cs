using FluentValidation;

namespace CrisisConnect.Application.UseCases.Transactions.AnnulerTransaction;

public class AnnulerTransactionCommandValidator : AbstractValidator<AnnulerTransactionCommand>
{
    public AnnulerTransactionCommandValidator()
    {
        RuleFor(x => x.TransactionId).NotEmpty();
    }
}
