using CrisisConnect.Application.UseCases.Propositions.ArchiverProposition;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class ArchiverPropositionCommandValidatorTests
{
    private readonly ArchiverPropositionCommandValidator _validator = new();

    [Fact]
    public void Valide_PropositionIdRempli_PasseValidation()
    {
        _validator.TestValidate(new ArchiverPropositionCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_PropositionIdVide_EchecSurPropositionId()
    {
        _validator.TestValidate(new ArchiverPropositionCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.PropositionId);
    }
}
