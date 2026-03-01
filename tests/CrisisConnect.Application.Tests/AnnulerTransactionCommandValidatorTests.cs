using CrisisConnect.Application.UseCases.Transactions.AnnulerTransaction;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class AnnulerTransactionCommandValidatorTests
{
    private readonly AnnulerTransactionCommandValidator _validator = new();

    [Fact]
    public void Valide_TransactionIdRempli_PasseValidation()
    {
        _validator.TestValidate(new AnnulerTransactionCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_TransactionIdVide_EchecSurTransactionId()
    {
        _validator.TestValidate(new AnnulerTransactionCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.TransactionId);
    }
}
