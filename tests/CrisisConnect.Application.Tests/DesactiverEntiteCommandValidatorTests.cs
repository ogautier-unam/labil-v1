using CrisisConnect.Application.UseCases.Entites.DesactiverEntite;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class DesactiverEntiteCommandValidatorTests
{
    private readonly DesactiverEntiteCommandValidator _validator = new();

    [Fact]
    public void Valide_EntiteIdRempli_PasseValidation()
    {
        _validator.TestValidate(new DesactiverEntiteCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_EntiteIdVide_EchecSurEntiteId()
    {
        _validator.TestValidate(new DesactiverEntiteCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.EntiteId);
    }
}
