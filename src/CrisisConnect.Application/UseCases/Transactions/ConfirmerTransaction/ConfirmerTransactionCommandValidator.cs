using FluentValidation;

namespace CrisisConnect.Application.UseCases.Transactions.ConfirmerTransaction;

public class ConfirmerTransactionCommandValidator : AbstractValidator<ConfirmerTransactionCommand>
{
    public ConfirmerTransactionCommandValidator()
    {
        RuleFor(x => x.TransactionId).NotEmpty();
    }
}
