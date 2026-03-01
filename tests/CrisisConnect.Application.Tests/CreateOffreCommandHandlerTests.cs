using CrisisConnect.Application.UseCases.Offres.CreateOffre;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class CreateOffreCommandHandlerTests
{
    private readonly IOffreRepository _offreRepo = Substitute.For<IOffreRepository>();
    private readonly IDemandeRepository _demandeRepo = Substitute.For<IDemandeRepository>();

    private CreateOffreCommandHandler CréerHandler() =>
        new(_offreRepo, _demandeRepo);

    [Fact]
    public async Task CreateOffre_SansLocalisation_CrééeEtRetournée()
    {
        // Arrange
        var command = new CreateOffreCommand("Titre offre", "Description", Guid.NewGuid());
        var handler = CréerHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Titre offre", result.Titre);
        Assert.Equal(StatutProposition.Active, result.Statut);
        await _offreRepo.Received(1).AddAsync(Arg.Any<Offre>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateOffre_AvecLivraison_LivraisonIncluse()
    {
        // Arrange
        var crePar = Guid.NewGuid();
        var command = new CreateOffreCommand("Titre", "Desc", crePar, LivraisonIncluse: true);
        var handler = CréerHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(crePar, result.CreePar);
        Assert.True(result.LivraisonIncluse);
    }

    [Fact]
    public async Task CreateOffre_AvecDemandesCouplées_IdsDansDto()
    {
        // Arrange
        var demande = new Demande("Besoin transport", "Transport bétail", Guid.NewGuid());
        _demandeRepo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>()).Returns(demande);

        var command = new CreateOffreCommand("Hébergement bovins", "Prêt de hangar", Guid.NewGuid(),
            DemandeIds: [demande.Id]);

        // Act
        var result = await CréerHandler().Handle(command, CancellationToken.None);

        // Assert
        Assert.Contains(demande.Id, result.DemandesCouplees);
        await _demandeRepo.Received(1).GetByIdAsync(demande.Id, Arg.Any<CancellationToken>());
    }
}
