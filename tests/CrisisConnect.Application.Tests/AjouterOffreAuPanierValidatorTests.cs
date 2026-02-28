using CrisisConnect.Application.UseCases.Paniers.AjouterOffreAuPanier;

namespace CrisisConnect.Application.Tests;

public class AjouterOffreAuPanierValidatorTests
{
    private readonly AjouterOffreAuPanierValidator _validator = new();

    [Fact]
    public async Task Valide_PanierIdEtOffreIdRemplis_PasseValidation()
    {
        var cmd = new AjouterOffreAuPanierCommand(Guid.NewGuid(), Guid.NewGuid());
        var result = await _validator.ValidateAsync(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Invalide_PanierIdVide_EchecSurPanierId()
    {
        var cmd = new AjouterOffreAuPanierCommand(Guid.Empty, Guid.NewGuid());
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PanierId");
    }

    [Fact]
    public async Task Invalide_OffreIdVide_EchecSurOffreId()
    {
        var cmd = new AjouterOffreAuPanierCommand(Guid.NewGuid(), Guid.Empty);
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "OffreId");
    }
}
