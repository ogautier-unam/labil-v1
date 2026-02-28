using CrisisConnect.Application.UseCases.Auth.RefreshToken;

namespace CrisisConnect.Application.Tests;

public class RefreshTokenValidatorTests
{
    private readonly RefreshTokenValidator _validator = new();

    [Fact]
    public async Task Valide_TokenRempli_PasseValidation()
    {
        var cmd = new RefreshTokenCommand("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.valid_token");
        var result = await _validator.ValidateAsync(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Invalide_TokenVide_EchecSurToken()
    {
        var cmd = new RefreshTokenCommand(string.Empty);
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Token");
    }

    [Fact]
    public async Task Invalide_TokenNull_EchecSurToken()
    {
        var cmd = new RefreshTokenCommand(null!);
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Token");
    }
}
