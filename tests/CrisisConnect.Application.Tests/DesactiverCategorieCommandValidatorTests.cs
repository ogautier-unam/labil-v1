using CrisisConnect.Application.UseCases.Taxonomie.DesactiverCategorie;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class DesactiverCategorieCommandValidatorTests
{
    private readonly DesactiverCategorieCommandValidator _validator = new();

    [Fact]
    public void Valide_CategorieIdRempli_PasseValidation()
    {
        _validator.TestValidate(new DesactiverCategorieCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_CategorieIdVide_EchecSurCategorieId()
    {
        _validator.TestValidate(new DesactiverCategorieCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.CategorieId);
    }
}
