using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Paniers.GetPanier;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetPanierQueryHandlerTests
{
    private readonly IPanierRepository _panierRepo = Substitute.For<IPanierRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetPanierQueryHandler CréerHandler() => new(_panierRepo, _mapper);

    [Fact]
    public async Task GetPanier_PanierOuvertExiste_RetourneLePanier()
    {
        // Arrange
        var proprietaireId = Guid.NewGuid();
        var panier = new Panier(proprietaireId);
        _panierRepo.GetByProprietaireAsync(proprietaireId, Arg.Any<CancellationToken>())
            .Returns(new List<Panier> { panier }.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetPanierQuery(proprietaireId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(proprietaireId, result.ProprietaireId);
        Assert.Equal(nameof(Domain.Enums.StatutPanier.Ouvert), result.Statut);
    }

    [Fact]
    public async Task GetPanier_AucunPanierOuvert_RetourneNull()
    {
        // Arrange
        var proprietaireId = Guid.NewGuid();
        var panierConfirmé = new Panier(proprietaireId);
        panierConfirmé.Confirmer();
        _panierRepo.GetByProprietaireAsync(proprietaireId, Arg.Any<CancellationToken>())
            .Returns(new List<Panier> { panierConfirmé }.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetPanierQuery(proprietaireId), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPanier_AucunPanier_RetourneNull()
    {
        // Arrange
        var proprietaireId = Guid.NewGuid();
        _panierRepo.GetByProprietaireAsync(proprietaireId, Arg.Any<CancellationToken>())
            .Returns(new List<Panier>().AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetPanierQuery(proprietaireId), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
