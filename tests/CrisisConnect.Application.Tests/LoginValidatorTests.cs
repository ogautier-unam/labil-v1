using CrisisConnect.Application.UseCases.Auth.Login;

namespace CrisisConnect.Application.Tests;

public class LoginValidatorTests
{
    private readonly LoginValidator _validator = new();

    [Fact]
    public async Task Valide_EmailEtMotDePasse_PasseValidation()
    {
        var cmd = new LoginCommand("user@example.com", "motdepasse");
        var result = await _validator.ValidateAsync(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Invalide_EmailVide_EchecSurEmail()
    {
        var cmd = new LoginCommand("", "motdepasse");
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Invalide_EmailMalFormé_EchecSurEmail()
    {
        var cmd = new LoginCommand("pas-un-email", "motdepasse");
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Invalide_MotDePasseVide_EchecSurMotDePasse()
    {
        var cmd = new LoginCommand("user@example.com", "");
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "MotDePasse");
    }
}
