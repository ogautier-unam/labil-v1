using CrisisConnect.Application.UseCases.Entites.CreateEntite;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class CreateEntiteValidatorTests
{
    private readonly CreateEntiteValidator _validator = new();

    private static CreateEntiteCommand CommandeValide() =>
        new("contact@ong.be", "motdepasse123", "Croix-Rouge", "Organisation humanitaire",
            "Tél: +32 2 345 67 89", Guid.NewGuid());

    [Fact]
    public void Valide_DonnéesComplètes_PasseValidation()
    {
        _validator.TestValidate(CommandeValide()).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_EmailMalFormé_EchecSurEmail()
    {
        var cmd = CommandeValide() with { Email = "pas-un-email" };
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Invalide_MotDePasseTropCourt_EchecSurMotDePasse()
    {
        var cmd = CommandeValide() with { MotDePasse = "court" };
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.MotDePasse);
    }

    [Fact]
    public void Invalide_NomVide_EchecSurNom()
    {
        var cmd = CommandeValide() with { Nom = "" };
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Nom);
    }

    [Fact]
    public void Invalide_ResponsableIdVide_EchecSurResponsableId()
    {
        var cmd = CommandeValide() with { ResponsableId = Guid.Empty };
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.ResponsableId);
    }
}
