using CrisisConnect.Application.UseCases.Propositions.MarquerEnAttenteRelance;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class MarquerEnAttenteRelanceCommandValidatorTests
{
    private readonly MarquerEnAttenteRelanceCommandValidator _validator = new();

    [Fact]
    public void Valide_PropositionIdRempli_PasseValidation()
    {
        _validator.TestValidate(new MarquerEnAttenteRelanceCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_PropositionIdVide_EchecSurPropositionId()
    {
        _validator.TestValidate(new MarquerEnAttenteRelanceCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.PropositionId);
    }
}
