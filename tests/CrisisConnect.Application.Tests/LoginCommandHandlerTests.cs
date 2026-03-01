using CrisisConnect.Application.UseCases.Auth.Login;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class LoginCommandHandlerTests
{
    private readonly IPersonneRepository _personneRepo = Substitute.For<IPersonneRepository>();
    private readonly IRefreshTokenRepository _refreshTokenRepo = Substitute.For<IRefreshTokenRepository>();
    private readonly IJwtService _jwtService = Substitute.For<IJwtService>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();

    private LoginCommandHandler CréerHandler() =>
        new(_personneRepo, _refreshTokenRepo, _jwtService, _passwordHasher);

    [Fact]
    public async Task Login_IdentifiantsValides_RetourneAuthResponse()
    {
        // Arrange
        var personne = new Personne("test@example.com", "hash", "Citoyen", "Jean", "Dupont");
        _personneRepo.GetByEmailAsync("test@example.com", Arg.Any<CancellationToken>())
            .Returns(personne);
        _passwordHasher.Verifier("motdepasse", "hash").Returns(true);
        _jwtService.GenererAccessToken(personne).Returns("access-token-123");
        _jwtService.GenererRefreshToken().Returns("refresh-token-xyz");

        var handler = CréerHandler();
        var command = new LoginCommand("test@example.com", "motdepasse");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(personne.Id, result.UserId);
        Assert.Equal("test@example.com", result.Email);
        Assert.Equal("Citoyen", result.Role);
        Assert.Equal("access-token-123", result.AccessToken);
        Assert.Equal("refresh-token-xyz", result.RefreshToken);
    }

    [Fact]
    public async Task Login_EmailInconnu_LèveUneDomainException()
    {
        // Arrange
        _personneRepo.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Personne?)null);

        var handler = CréerHandler();
        var command = new LoginCommand("inconnu@example.com", "motdepasse");

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None).AsTask());
    }

    [Fact]
    public async Task Login_MotDePasseIncorrect_LèveUneDomainException()
    {
        // Arrange
        var personne = new Personne("test@example.com", "hash", "Citoyen", "Jean", "Dupont");
        _personneRepo.GetByEmailAsync("test@example.com", Arg.Any<CancellationToken>())
            .Returns(personne);
        _passwordHasher.Verifier("mauvais", "hash").Returns(false);

        var handler = CréerHandler();
        var command = new LoginCommand("test@example.com", "mauvais");

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None).AsTask());
    }

    [Fact]
    public async Task Login_ConnexionRéussie_RévoqueLesAncienTokens()
    {
        // Arrange
        var personne = new Personne("test@example.com", "hash", "Citoyen", "Jean", "Dupont");
        _personneRepo.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(personne);
        _passwordHasher.Verifier(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
        _jwtService.GenererAccessToken(Arg.Any<Personne>()).Returns("tok");
        _jwtService.GenererRefreshToken().Returns("ref");

        var handler = CréerHandler();

        // Act
        await handler.Handle(new LoginCommand("test@example.com", "ok"), CancellationToken.None);

        // Assert
        await _refreshTokenRepo.Received(1).RevoquerTousAsync(personne.Id, Arg.Any<CancellationToken>());
    }
}
