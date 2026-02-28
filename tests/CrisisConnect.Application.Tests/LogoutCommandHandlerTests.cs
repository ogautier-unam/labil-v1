using CrisisConnect.Application.UseCases.Auth.Logout;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class LogoutCommandHandlerTests
{
    private readonly IRefreshTokenRepository _refreshTokenRepo = Substitute.For<IRefreshTokenRepository>();

    private LogoutCommandHandler CréerHandler() => new(_refreshTokenRepo);

    [Fact]
    public async Task Logout_AppelleRevoquerTousAvecLePersonneId()
    {
        // Arrange
        var personneId = Guid.NewGuid();

        // Act
        await CréerHandler().Handle(new LogoutCommand(personneId), CancellationToken.None);

        // Assert
        await _refreshTokenRepo.Received(1).RevoquerTousAsync(personneId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Logout_PersonneIdVide_AppelleQuandMêmeRevoquerTous()
    {
        // Arrange — le handler délègue sans validation (validée en amont par le controller)
        var personneId = Guid.Empty;

        // Act
        await CréerHandler().Handle(new LogoutCommand(personneId), CancellationToken.None);

        // Assert
        await _refreshTokenRepo.Received(1).RevoquerTousAsync(personneId, Arg.Any<CancellationToken>());
    }
}
