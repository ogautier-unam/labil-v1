using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Journal.GetEntreesJournal;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetEntreesJournalQueryHandlerTests
{
    private readonly IEntreeJournalRepository _journalRepo = Substitute.For<IEntreeJournalRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetEntreesJournalQueryHandler CréerHandler() => new(_journalRepo, _mapper);

    [Fact]
    public async Task GetEntreesJournal_DeuxEntrées_RetourneDeuxDtos()
    {
        // Arrange
        var acteurId = Guid.NewGuid();
        var entrees = new List<EntreeJournal>
        {
            new(acteurId, TypeOperation.Connexion, details: "LoginCommand"),
            new(acteurId, TypeOperation.DepotProposition, details: "CreateOffreCommand")
        };
        _journalRepo.GetByActeurAsync(acteurId, Arg.Any<CancellationToken>())
            .Returns(entrees.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetEntreesJournalQuery(acteurId), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, e => e.TypeOperation == TypeOperation.Connexion);
        Assert.Contains(result, e => e.TypeOperation == TypeOperation.DepotProposition);
    }

    [Fact]
    public async Task GetEntreesJournal_AucuneEntrée_RetourneListeVide()
    {
        // Arrange
        var acteurId = Guid.NewGuid();
        _journalRepo.GetByActeurAsync(acteurId, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<EntreeJournal>());

        // Act
        var result = await CréerHandler().Handle(new GetEntreesJournalQuery(acteurId), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetEntreesJournal_ActeurId_TransmisAuRepository()
    {
        // Arrange
        var acteurId = Guid.NewGuid();
        _journalRepo.GetByActeurAsync(acteurId, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<EntreeJournal>());

        // Act
        await CréerHandler().Handle(new GetEntreesJournalQuery(acteurId), CancellationToken.None);

        // Assert — le repository est interrogé avec le bon identifiant
        await _journalRepo.Received(1).GetByActeurAsync(acteurId, Arg.Any<CancellationToken>());
    }
}
