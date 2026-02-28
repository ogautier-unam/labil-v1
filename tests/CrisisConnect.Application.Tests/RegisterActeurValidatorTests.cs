using CrisisConnect.Application.UseCases.Auth.Register;

namespace CrisisConnect.Application.Tests;

public class RegisterActeurValidatorTests
{
    private readonly RegisterActeurValidator _validator = new();

    [Fact]
    public async Task Valide_DonnéesComplètes_PasseValidation()
    {
        var cmd = new RegisterActeurCommand("alice@example.com", "motdepasse123", "Citoyen", "Alice", "Martin");
        var result = await _validator.ValidateAsync(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Invalide_EmailMalFormé_EchecSurEmail()
    {
        var cmd = new RegisterActeurCommand("pasunemail", "motdepasse123", "Citoyen", "Alice", "Martin");
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Invalide_MotDePasseTropCourt_EchecSurMotDePasse()
    {
        var cmd = new RegisterActeurCommand("alice@example.com", "court", "Citoyen", "Alice", "Martin");
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "MotDePasse");
    }

    [Fact]
    public async Task Invalide_RôleInconnu_EchecSurRole()
    {
        var cmd = new RegisterActeurCommand("alice@example.com", "motdepasse123", "SuperAdmin", "Alice", "Martin");
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Role");
    }

    [Fact]
    public async Task Invalide_PrenomVide_EchecSurPrenom()
    {
        var cmd = new RegisterActeurCommand("alice@example.com", "motdepasse123", "Benevole", "", "Martin");
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Prenom");
    }
}
