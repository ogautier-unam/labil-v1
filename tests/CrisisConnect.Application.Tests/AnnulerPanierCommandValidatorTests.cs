using CrisisConnect.Application.UseCases.Paniers.AnnulerPanier;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class AnnulerPanierCommandValidatorTests
{
    private readonly AnnulerPanierCommandValidator _validator = new();

    [Fact]
    public void Valide_PanierIdRempli_PasseValidation()
    {
        _validator.TestValidate(new AnnulerPanierCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_PanierIdVide_EchecSurPanierId()
    {
        _validator.TestValidate(new AnnulerPanierCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.PanierId);
    }
}
