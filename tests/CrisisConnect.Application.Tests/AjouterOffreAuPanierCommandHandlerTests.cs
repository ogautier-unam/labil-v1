using AutoMapper;
using CrisisConnect.Application.UseCases.Paniers.AjouterOffreAuPanier;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class AjouterOffreAuPanierCommandHandlerTests
{
    private readonly IPanierRepository _panierRepo = Substitute.For<IPanierRepository>();
    private readonly IOffreRepository _offreRepo = Substitute.For<IOffreRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private AjouterOffreAuPanierCommandHandler CréerHandler() =>
        new(_panierRepo, _offreRepo, _mapper);

    [Fact]
    public async Task AjouterOffre_PanierEtOffreExistants_OffreAjoutée()
    {
        // Arrange
        var panier = new Panier(Guid.NewGuid());
        var offre = new Offre("Titre", "Desc", Guid.NewGuid());

        _panierRepo.GetByIdAsync(panier.Id, Arg.Any<CancellationToken>()).Returns(panier);
        _offreRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>()).Returns(offre);

        var handler = CréerHandler();
        var command = new AjouterOffreAuPanierCommand(panier.Id, offre.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Single(result.Offres);
        Assert.Equal(offre.Id, result.Offres[0].Id);
        await _panierRepo.Received(1).UpdateAsync(panier, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AjouterOffre_PanierIntrouvable_LèveNotFoundException()
    {
        // Arrange
        _panierRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Panier?)null);

        var handler = CréerHandler();
        var command = new AjouterOffreAuPanierCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task AjouterOffre_OffreIntrouvable_LèveNotFoundException()
    {
        // Arrange
        var panier = new Panier(Guid.NewGuid());
        _panierRepo.GetByIdAsync(panier.Id, Arg.Any<CancellationToken>()).Returns(panier);
        _offreRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Offre?)null);

        var handler = CréerHandler();
        var command = new AjouterOffreAuPanierCommand(panier.Id, Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}
