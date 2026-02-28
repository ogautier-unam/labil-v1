using CrisisConnect.Domain.Entities;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class SuggestionAppariementRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetByIdAsync_RetourneSuggestion()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new SuggestionAppariementRepository(ctx);

        var suggestion = new SuggestionAppariement(Guid.NewGuid(), Guid.NewGuid(), 0.87, "Correspondance géographique + catégorie");
        await repo.AddAsync(suggestion);

        var résultat = await repo.GetByIdAsync(suggestion.Id);

        Assert.NotNull(résultat);
        Assert.Equal(0.87, résultat.ScoreCorrespondance);
        Assert.False(résultat.EstAcknowledged);
    }

    [Fact]
    public async Task GetByOffreAsync_FiltreParOffre()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new SuggestionAppariementRepository(ctx);

        var offreId = Guid.NewGuid();
        await repo.AddAsync(new SuggestionAppariement(offreId, Guid.NewGuid(), 0.9, "Correspondance A"));
        await repo.AddAsync(new SuggestionAppariement(offreId, Guid.NewGuid(), 0.7, "Correspondance B"));
        await repo.AddAsync(new SuggestionAppariement(Guid.NewGuid(), Guid.NewGuid(), 0.5, "Autre offre"));

        var résultats = await repo.GetByOffreAsync(offreId);

        Assert.Equal(2, résultats.Count);
        Assert.All(résultats, s => Assert.Equal(offreId, s.OffreId));
        // Doit être trié par score décroissant
        Assert.True(résultats[0].ScoreCorrespondance >= résultats[1].ScoreCorrespondance);
    }

    [Fact]
    public async Task GetNonAcknowledgedAsync_RetourneSeulementNonAcknowledged()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new SuggestionAppariementRepository(ctx);

        var s1 = new SuggestionAppariement(Guid.NewGuid(), Guid.NewGuid(), 0.8, "Non ack");
        var s2 = new SuggestionAppariement(Guid.NewGuid(), Guid.NewGuid(), 0.6, "Sera ack");
        await repo.AddAsync(s1);
        await repo.AddAsync(s2);

        s2.Acknowledger();
        await repo.UpdateAsync(s2);

        var résultats = await repo.GetNonAcknowledgedAsync();

        Assert.Single(résultats);
        Assert.Equal(s1.Id, résultats[0].Id);
    }

    [Fact]
    public async Task UpdateAsync_Acknowledger_EstAcknowledgedPersisté()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new SuggestionAppariementRepository(ctx);

        var suggestion = new SuggestionAppariement(Guid.NewGuid(), Guid.NewGuid(), 0.75, "Test ack");
        await repo.AddAsync(suggestion);

        suggestion.Acknowledger();
        await repo.UpdateAsync(suggestion);

        var chargée = await repo.GetByIdAsync(suggestion.Id);
        Assert.True(chargée!.EstAcknowledged);
    }
}
