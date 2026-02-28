using CrisisConnect.Application.UseCases.Offres.CreateOffre;

namespace CrisisConnect.Application.Tests;

public class CreateOffreValidatorTests
{
    private readonly CreateOffreValidator _validator = new();

    [Fact]
    public async Task Valide_DonnéesComplètes_PasseValidation()
    {
        var cmd = new CreateOffreCommand("Matériel médical", "Don de masques FFP2", Guid.NewGuid());
        var result = await _validator.ValidateAsync(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Invalide_TitreVide_EchecSurTitre()
    {
        var cmd = new CreateOffreCommand("", "Description valide", Guid.NewGuid());
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Titre");
    }

    [Fact]
    public async Task Invalide_DescriptionVide_EchecSurDescription()
    {
        var cmd = new CreateOffreCommand("Titre valide", "", Guid.NewGuid());
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Description");
    }

    [Fact]
    public async Task Invalide_CréeParVide_EchecSurCreePar()
    {
        var cmd = new CreateOffreCommand("Titre valide", "Description valide", Guid.Empty);
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CreePar");
    }
}
