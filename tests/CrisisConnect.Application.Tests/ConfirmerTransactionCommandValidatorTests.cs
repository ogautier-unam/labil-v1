using CrisisConnect.Application.UseCases.Transactions.ConfirmerTransaction;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class ConfirmerTransactionCommandValidatorTests
{
    private readonly ConfirmerTransactionCommandValidator _validator = new();

    [Fact]
    public void Valide_TransactionIdRempli_PasseValidation()
    {
        _validator.TestValidate(new ConfirmerTransactionCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_TransactionIdVide_EchecSurTransactionId()
    {
        _validator.TestValidate(new ConfirmerTransactionCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.TransactionId);
    }
}
