using CrisisConnect.Application.UseCases.Propositions.ReconfirmerProposition;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class ReconfirmerPropositionCommandValidatorTests
{
    private readonly ReconfirmerPropositionCommandValidator _validator = new();

    [Fact]
    public void Valide_PropositionIdRempli_PasseValidation()
    {
        _validator.TestValidate(new ReconfirmerPropositionCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_PropositionIdVide_EchecSurPropositionId()
    {
        _validator.TestValidate(new ReconfirmerPropositionCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.PropositionId);
    }
}
