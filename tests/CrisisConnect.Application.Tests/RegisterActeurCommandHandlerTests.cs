using CrisisConnect.Application.UseCases.Auth.Register;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class RegisterActeurCommandHandlerTests
{
    private readonly IPersonneRepository _personneRepo = Substitute.For<IPersonneRepository>();
    private readonly IRefreshTokenRepository _refreshTokenRepo = Substitute.For<IRefreshTokenRepository>();
    private readonly IJwtService _jwtService = Substitute.For<IJwtService>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();

    private RegisterActeurCommandHandler CréerHandler() =>
        new(_personneRepo, _refreshTokenRepo, _jwtService, _passwordHasher);

    [Fact]
    public async Task Register_EmailInconnu_CrééEtRetourneAuthResponse()
    {
        // Arrange
        _personneRepo.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((Personne?)null);
        _passwordHasher.Hacher(Arg.Any<string>()).Returns("hash_securise");
        _jwtService.GenererAccessToken(Arg.Any<Personne>()).Returns("access_token_jwt");
        _jwtService.GenererRefreshToken().Returns("refresh_token");

        var command = new RegisterActeurCommand("alice@example.com", "MotDePasse1!", "Citoyen", "Alice", "Durand");
        var handler = CréerHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("alice@example.com", result.Email);
        Assert.Equal("Citoyen", result.Role);
        Assert.Equal("access_token_jwt", result.AccessToken);
        await _personneRepo.Received(1).AddAsync(Arg.Any<Personne>(), Arg.Any<CancellationToken>());
        await _refreshTokenRepo.Received(1).AddAsync(Arg.Any<RefreshToken>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Register_EmailDéjàUtilisé_LèveUneDomainException()
    {
        // Arrange
        var existant = new Personne("alice@example.com", "hash", "Citoyen", "Alice", "Durand");
        _personneRepo.GetByEmailAsync("alice@example.com", Arg.Any<CancellationToken>())
            .Returns(existant);

        var command = new RegisterActeurCommand("alice@example.com", "MotDePasse1!", "Citoyen", "Alice", "Durand");
        var handler = CréerHandler();

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None).AsTask());
        await _personneRepo.DidNotReceive().AddAsync(Arg.Any<Personne>(), Arg.Any<CancellationToken>());
    }
}
