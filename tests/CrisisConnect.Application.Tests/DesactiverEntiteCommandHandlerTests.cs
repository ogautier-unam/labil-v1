using CrisisConnect.Application.UseCases.Entites.DesactiverEntite;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class DesactiverEntiteCommandHandlerTests
{
    private readonly IEntiteRepository _entiteRepo = Substitute.For<IEntiteRepository>();

    private DesactiverEntiteCommandHandler CréerHandler() => new(_entiteRepo);

    [Fact]
    public async Task DesactiverEntite_EntiteExiste_DesactivéeEtPersistée()
    {
        var entite = new Entite("org@example.com", "hash", "Croix-Rouge", "Org humanitaire", "+32 2 345 67 89", Guid.NewGuid());
        Assert.True(entite.EstActive);
        _entiteRepo.GetByIdAsync(entite.Id, Arg.Any<CancellationToken>()).Returns(entite);

        await CréerHandler().Handle(new DesactiverEntiteCommand(entite.Id), CancellationToken.None);

        Assert.False(entite.EstActive);
        await _entiteRepo.Received(1).UpdateAsync(entite, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DesactiverEntite_EntiteIntrouvable_LèveNotFoundException()
    {
        var id = Guid.NewGuid();
        _entiteRepo.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Entite?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new DesactiverEntiteCommand(id), CancellationToken.None));
    }
}
