using CrisisConnect.Application.UseCases.MethodesIdentification.VerifierMethode;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class VerifierMethodeCommandValidatorTests
{
    private readonly VerifierMethodeCommandValidator _validator = new();

    [Fact]
    public void Valide_MethodeIdRempli_PasseValidation()
    {
        _validator.TestValidate(new VerifierMethodeCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_MethodeIdVide_EchecSurMethodeId()
    {
        _validator.TestValidate(new VerifierMethodeCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.MethodeId);
    }
}
