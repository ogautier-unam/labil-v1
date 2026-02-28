using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class EntreeJournalRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetByActeurAsync_RetourneEntree()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new EntreeJournalRepository(ctx);

        var acteurId = Guid.NewGuid();
        var entree = new EntreeJournal(acteurId, TypeOperation.Connexion, details: "Connexion réussie");
        await repo.AddAsync(entree);

        var résultats = await repo.GetByActeurAsync(acteurId);

        Assert.Single(résultats);
        Assert.Equal(TypeOperation.Connexion, résultats[0].TypeOperation);
        Assert.Equal("Connexion réussie", résultats[0].Details);
    }

    [Fact]
    public async Task GetByActeurAsync_FiltreParActeur()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new EntreeJournalRepository(ctx);

        var acteur1 = Guid.NewGuid();
        var acteur2 = Guid.NewGuid();
        await repo.AddAsync(new EntreeJournal(acteur1, TypeOperation.DepotProposition));
        await repo.AddAsync(new EntreeJournal(acteur1, TypeOperation.DebutTransaction));
        await repo.AddAsync(new EntreeJournal(acteur2, TypeOperation.Connexion));

        var résultats = await repo.GetByActeurAsync(acteur1);

        Assert.Equal(2, résultats.Count);
        Assert.All(résultats, e => Assert.Equal(acteur1, e.ActeurId));
    }

    [Fact]
    public async Task GetByActeurAsync_ActeurSansEntrees_RetourneListeVide()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new EntreeJournalRepository(ctx);

        var résultats = await repo.GetByActeurAsync(Guid.NewGuid());

        Assert.Empty(résultats);
    }
}
