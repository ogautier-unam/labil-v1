using CrisisConnect.Application.UseCases.Transactions.BasculerVisibiliteDiscussion;
using CrisisConnect.Domain.Enums;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class BasculerVisibiliteDiscussionCommandValidatorTests
{
    private readonly BasculerVisibiliteDiscussionCommandValidator _validator = new();

    [Fact]
    public void Valide_TransactionIdRempli_PasseValidation()
    {
        _validator.TestValidate(new BasculerVisibiliteDiscussionCommand(Guid.NewGuid(), Visibilite.Privee))
            .ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_TransactionIdVide_EchecSurTransactionId()
    {
        _validator.TestValidate(new BasculerVisibiliteDiscussionCommand(Guid.Empty, Visibilite.Publique))
            .ShouldHaveValidationErrorFor(x => x.TransactionId);
    }
}
