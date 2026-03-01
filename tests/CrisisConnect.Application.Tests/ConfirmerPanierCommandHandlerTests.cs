using CrisisConnect.Application.UseCases.Paniers.ConfirmerPanier;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class ConfirmerPanierCommandHandlerTests
{
    private readonly IPanierRepository _panierRepo = Substitute.For<IPanierRepository>();

    private ConfirmerPanierCommandHandler CréerHandler() => new(_panierRepo);

    [Fact]
    public async Task ConfirmerPanier_PanierExiste_ConfirméEtPersisté()
    {
        // Arrange
        var panier = new Panier(Guid.NewGuid());
        _panierRepo.GetByIdAsync(panier.Id, Arg.Any<CancellationToken>())
            .Returns(panier);

        // Act
        await CréerHandler().Handle(new ConfirmerPanierCommand(panier.Id), CancellationToken.None);

        // Assert
        await _panierRepo.Received(1).UpdateAsync(panier, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ConfirmerPanier_PanierIntrouvable_LèveNotFoundException()
    {
        // Arrange
        var panierId = Guid.NewGuid();
        _panierRepo.GetByIdAsync(panierId, Arg.Any<CancellationToken>())
            .Returns((Panier?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new ConfirmerPanierCommand(panierId), CancellationToken.None).AsTask());
    }
}
