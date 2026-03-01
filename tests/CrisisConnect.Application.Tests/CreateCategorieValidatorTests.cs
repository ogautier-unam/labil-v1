using CrisisConnect.Application.UseCases.Taxonomie.CreateCategorie;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class CreateCategorieValidatorTests
{
    private readonly CreateCategorieValidator _validator = new();

    [Fact]
    public void Valide_DonnéesComplètes_PasseValidation()
    {
        var cmd = new CreateCategorieCommand("LOGEMENT", "{\"fr\":\"Logement\"}", Guid.NewGuid());
        _validator.TestValidate(cmd).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_CodeVide_EchecSurCode()
    {
        var cmd = new CreateCategorieCommand("", "{\"fr\":\"Logement\"}", Guid.NewGuid());
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    public void Invalide_NomJsonVide_EchecSurNomJson()
    {
        var cmd = new CreateCategorieCommand("LOGEMENT", "", Guid.NewGuid());
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.NomJson);
    }

    [Fact]
    public void Invalide_ConfigIdVide_EchecSurConfigId()
    {
        var cmd = new CreateCategorieCommand("LOGEMENT", "{\"fr\":\"Logement\"}", Guid.Empty);
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.ConfigId);
    }
}
