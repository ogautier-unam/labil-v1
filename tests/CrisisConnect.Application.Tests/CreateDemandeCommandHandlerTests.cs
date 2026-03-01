using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Demandes.CreateDemande;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class CreateDemandeCommandHandlerTests
{
    private readonly IDemandeRepository _demandeRepo = Substitute.For<IDemandeRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private CreateDemandeCommandHandler CréerHandler() =>
        new(_demandeRepo, _mapper);

    [Fact]
    public async Task CreateDemande_ParDéfaut_CrééeAvecUrgenceMoyenne()
    {
        // Arrange
        var command = new CreateDemandeCommand("Besoin eau", "Description", Guid.NewGuid());
        var handler = CréerHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Besoin eau", result.Titre);
        Assert.Equal(NiveauUrgence.Moyen, result.Urgence);
        Assert.Equal(StatutProposition.Active, result.Statut);
        await _demandeRepo.Received(1).AddAsync(Arg.Any<Demande>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateDemande_UrgenceCritique_RetourneUrgenceCritique()
    {
        // Arrange
        var command = new CreateDemandeCommand(
            "Urgence médicale", "Besoin médecin", Guid.NewGuid(),
            Urgence: NiveauUrgence.Critique,
            RegionSeverite: "Zone A");
        var handler = CréerHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(NiveauUrgence.Critique, result.Urgence);
        Assert.Equal("Zone A", result.RegionSeverite);
    }
}
