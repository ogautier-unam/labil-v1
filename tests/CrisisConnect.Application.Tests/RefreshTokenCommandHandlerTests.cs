using CrisisConnect.Application.UseCases.Auth.RefreshToken;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class RefreshTokenCommandHandlerTests
{
    private readonly IRefreshTokenRepository _refreshTokenRepo = Substitute.For<IRefreshTokenRepository>();
    private readonly IPersonneRepository _personneRepo = Substitute.For<IPersonneRepository>();
    private readonly IJwtService _jwtService = Substitute.For<IJwtService>();

    private RefreshTokenCommandHandler CréerHandler() =>
        new(_refreshTokenRepo, _personneRepo, _jwtService);

    [Fact]
    public async Task RefreshToken_TokenValide_RetourneNouveauxTokens()
    {
        // Arrange
        var personneId = Guid.NewGuid();
        var token = new RefreshToken("valid-token", personneId, DateTime.UtcNow.AddDays(7));
        var personne = new Personne("alice@example.com", "hash", "Citoyen", "Alice", "Martin");

        _refreshTokenRepo.GetByTokenAsync("valid-token", Arg.Any<CancellationToken>())
            .Returns(token);
        _personneRepo.GetByIdAsync(personneId, Arg.Any<CancellationToken>())
            .Returns(personne);
        _jwtService.GenererAccessToken(personne).Returns("nouveau-access-token");
        _jwtService.GenererRefreshToken().Returns("nouveau-refresh-token");

        // Act
        var result = await CréerHandler().Handle(new RefreshTokenCommand("valid-token"), CancellationToken.None);

        // Assert
        Assert.Equal("nouveau-access-token", result.AccessToken);
        Assert.Equal("nouveau-refresh-token", result.RefreshToken);
        await _refreshTokenRepo.Received(1).UpdateAsync(token, Arg.Any<CancellationToken>());
        await _refreshTokenRepo.Received(1).AddAsync(Arg.Any<RefreshToken>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RefreshToken_TokenExpiré_LèveDomainException()
    {
        // Arrange
        var tokenExpiré = new RefreshToken("expired-token", Guid.NewGuid(), DateTime.UtcNow.AddDays(-1));
        _refreshTokenRepo.GetByTokenAsync("expired-token", Arg.Any<CancellationToken>())
            .Returns(tokenExpiré);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() =>
            CréerHandler().Handle(new RefreshTokenCommand("expired-token"), CancellationToken.None));
    }

    [Fact]
    public async Task RefreshToken_TokenIntrouvable_LèveDomainException()
    {
        // Arrange
        _refreshTokenRepo.GetByTokenAsync("inconnu", Arg.Any<CancellationToken>())
            .Returns((RefreshToken?)null);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() =>
            CréerHandler().Handle(new RefreshTokenCommand("inconnu"), CancellationToken.None));
    }
}
