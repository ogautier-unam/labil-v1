using CrisisConnect.Application.UseCases.Offres.UpdateOffre;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class UpdateOffreCommandHandlerTests
{
    private readonly IOffreRepository _offreRepo = Substitute.For<IOffreRepository>();

    private UpdateOffreCommandHandler CréerHandler() => new(_offreRepo);

    [Fact]
    public async Task UpdateOffre_OffreActive_MiseAJourEtRetournée()
    {
        // Arrange
        var offre = new Offre("Titre initial", "Description initiale", Guid.NewGuid());
        _offreRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>()).Returns(offre);

        var cmd = new UpdateOffreCommand(offre.Id, "Nouveau titre", "Nouvelle description", true, 45.0, 2.5);

        // Act
        var result = await CréerHandler().Handle(cmd, CancellationToken.None);

        // Assert
        Assert.Equal("Nouveau titre", result.Titre);
        Assert.Equal("Nouvelle description", result.Description);
        Assert.True(result.LivraisonIncluse);
        await _offreRepo.Received(1).UpdateAsync(offre, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateOffre_OffreIntrouvable_LèveNotFoundException()
    {
        // Arrange
        _offreRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Offre?)null);

        var cmd = new UpdateOffreCommand(Guid.NewGuid(), "Titre", "Description");

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => CréerHandler().Handle(cmd, CancellationToken.None).AsTask());
    }

    [Fact]
    public async Task UpdateOffre_OffreEnTransaction_LèveDomainException()
    {
        // Arrange
        var offre = new Offre("Titre", "Description", Guid.NewGuid());
        offre.MarquerEnTransaction();
        _offreRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>()).Returns(offre);

        var cmd = new UpdateOffreCommand(offre.Id, "Nouveau titre", "Nouvelle description");

        // Act & Assert
        await Assert.ThrowsAsync<Domain.Exceptions.DomainException>(
            () => CréerHandler().Handle(cmd, CancellationToken.None).AsTask());
        await _offreRepo.DidNotReceive().UpdateAsync(Arg.Any<Offre>(), Arg.Any<CancellationToken>());
    }
}
