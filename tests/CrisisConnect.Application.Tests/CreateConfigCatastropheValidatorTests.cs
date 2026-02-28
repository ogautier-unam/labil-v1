using CrisisConnect.Application.UseCases.ConfigCatastrophe.CreateConfigCatastrophe;

namespace CrisisConnect.Application.Tests;

public class CreateConfigCatastropheValidatorTests
{
    private readonly CreateConfigCatastropheValidator _validator = new();

    [Fact]
    public async Task Valide_DonnéesComplètes_PasseValidation()
    {
        var cmd = new CreateConfigCatastropheCommand("Inondation", "Crise PACA", "PACA", "France", 30, 7);
        var result = await _validator.ValidateAsync(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Invalide_NomVide_EchecSurNom()
    {
        var cmd = new CreateConfigCatastropheCommand("", "Desc", "Zone", "Etat", 30, 7);
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Nom");
    }

    [Fact]
    public async Task Invalide_DelaiArchivageZero_EchecSurDelai()
    {
        var cmd = new CreateConfigCatastropheCommand("Nom", "Desc", "Zone", "Etat", 0, 7);
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "DelaiArchivageJours");
    }

    [Fact]
    public async Task Invalide_RappelSuperiorAuArchivage_EchecSurRappel()
    {
        // DelaiRappel (20) doit être < DelaiArchivage (10) — invalide
        var cmd = new CreateConfigCatastropheCommand("Nom", "Desc", "Zone", "Etat", 10, 20);
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "DelaiRappelAvantArchivage");
    }
}
