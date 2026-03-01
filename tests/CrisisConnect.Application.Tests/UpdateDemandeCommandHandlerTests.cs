using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Demandes.UpdateDemande;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class UpdateDemandeCommandHandlerTests
{
    private readonly IDemandeRepository _demandeRepo = Substitute.For<IDemandeRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private UpdateDemandeCommandHandler CréerHandler() => new(_demandeRepo, _mapper);

    [Fact]
    public async Task UpdateDemande_DemandeActive_MiseAJourEtRetournée()
    {
        // Arrange
        var demande = new Demande("Titre initial", "Description initiale", Guid.NewGuid());
        _demandeRepo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>()).Returns(demande);

        var cmd = new UpdateDemandeCommand(demande.Id, "Nouveau titre", "Nouvelle description",
            NiveauUrgence.Critique, "Nord-Est");

        // Act
        var result = await CréerHandler().Handle(cmd, CancellationToken.None);

        // Assert
        Assert.Equal("Nouveau titre", result.Titre);
        Assert.Equal("Nouvelle description", result.Description);
        Assert.Equal(NiveauUrgence.Critique, result.Urgence);
        Assert.Equal("Nord-Est", result.RegionSeverite);
        await _demandeRepo.Received(1).UpdateAsync(demande, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateDemande_DemandeIntrouvable_LèveNotFoundException()
    {
        // Arrange
        _demandeRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Demande?)null);

        var cmd = new UpdateDemandeCommand(Guid.NewGuid(), "Titre", "Description");

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => CréerHandler().Handle(cmd, CancellationToken.None).AsTask());
    }
}
