using CrisisConnect.Application.UseCases.Paniers.CreatePanier;

namespace CrisisConnect.Application.Tests;

public class CreatePanierValidatorTests
{
    private readonly CreatePanierValidator _validator = new();

    [Fact]
    public async Task Valide_ProprietaireIdRempli_PasseValidation()
    {
        var cmd = new CreatePanierCommand(Guid.NewGuid());
        var result = await _validator.ValidateAsync(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Invalide_ProprietaireIdVide_EchecSurProprietaireId()
    {
        var cmd = new CreatePanierCommand(Guid.Empty);
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ProprietaireId");
    }
}
