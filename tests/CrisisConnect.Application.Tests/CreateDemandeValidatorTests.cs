using CrisisConnect.Application.UseCases.Demandes.CreateDemande;
using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.Tests;

public class CreateDemandeValidatorTests
{
    private readonly CreateDemandeValidator _validator = new();

    [Fact]
    public async Task Valide_DonnéesComplètes_PasseValidation()
    {
        var cmd = new CreateDemandeCommand("Besoin en eau potable", "Urgence hydrique secteur nord", Guid.NewGuid(),
            Urgence: NiveauUrgence.Eleve);
        var result = await _validator.ValidateAsync(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Invalide_TitreVide_EchecSurTitre()
    {
        var cmd = new CreateDemandeCommand("", "Description valide", Guid.NewGuid());
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Titre");
    }

    [Fact]
    public async Task Invalide_CréeParVide_EchecSurCreePar()
    {
        var cmd = new CreateDemandeCommand("Titre valide", "Description valide", Guid.Empty);
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CreePar");
    }

    [Fact]
    public async Task Invalide_DescriptionVide_EchecSurDescription()
    {
        var cmd = new CreateDemandeCommand("Titre valide", "", Guid.NewGuid());
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Description");
    }
}
