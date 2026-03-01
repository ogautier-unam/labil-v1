using CrisisConnect.Application.UseCases.Propositions.CloreProposition;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class ClorePropositionCommandValidatorTests
{
    private readonly ClorePropositionCommandValidator _validator = new();

    [Fact]
    public void Valide_PropositionIdRempli_PasseValidation()
    {
        _validator.TestValidate(new ClorePropositionCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_PropositionIdVide_EchecSurPropositionId()
    {
        _validator.TestValidate(new ClorePropositionCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.PropositionId);
    }
}
