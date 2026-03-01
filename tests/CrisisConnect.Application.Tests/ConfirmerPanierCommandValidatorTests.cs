using CrisisConnect.Application.UseCases.Paniers.ConfirmerPanier;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class ConfirmerPanierCommandValidatorTests
{
    private readonly ConfirmerPanierCommandValidator _validator = new();

    [Fact]
    public void Valide_PanierIdRempli_PasseValidation()
    {
        _validator.TestValidate(new ConfirmerPanierCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_PanierIdVide_EchecSurPanierId()
    {
        _validator.TestValidate(new ConfirmerPanierCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.PanierId);
    }
}
