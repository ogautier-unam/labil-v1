using CrisisConnect.Application.UseCases.Paniers.AnnulerPanier;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class AnnulerPanierCommandHandlerTests
{
    private readonly IPanierRepository _panierRepo = Substitute.For<IPanierRepository>();

    private AnnulerPanierCommandHandler CréerHandler() => new(_panierRepo);

    [Fact]
    public async Task AnnulerPanier_PanierExiste_AnnuléEtPersisté()
    {
        // Arrange
        var panier = new Panier(Guid.NewGuid());
        _panierRepo.GetByIdAsync(panier.Id, Arg.Any<CancellationToken>())
            .Returns(panier);

        // Act
        await CréerHandler().Handle(new AnnulerPanierCommand(panier.Id), CancellationToken.None);

        // Assert
        await _panierRepo.Received(1).UpdateAsync(panier, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AnnulerPanier_PanierIntrouvable_LèveNotFoundException()
    {
        // Arrange
        var panierId = Guid.NewGuid();
        _panierRepo.GetByIdAsync(panierId, Arg.Any<CancellationToken>())
            .Returns((Panier?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new AnnulerPanierCommand(panierId), CancellationToken.None).AsTask());
    }
}
