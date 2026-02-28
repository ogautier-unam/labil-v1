using CrisisConnect.Domain.Entities;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class EntiteRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetByIdAsync_RetourneEntite()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new EntiteRepository(ctx);

        var entite = new Entite("asso@example.com", "hash", "Croix-Rouge", "Aide humanitaire", "0600000000", Guid.NewGuid());
        await repo.AddAsync(entite);

        var résultat = await repo.GetByIdAsync(entite.Id);

        Assert.NotNull(résultat);
        Assert.Equal("Croix-Rouge", résultat.Nom);
        Assert.True(résultat.EstActive);
    }

    [Fact]
    public async Task GetAllAsync_DeuxEntites_RetourneLesDeux()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new EntiteRepository(ctx);

        await repo.AddAsync(new Entite("e1@example.com", "hash", "Entite 1", "Desc 1", "0600000001", Guid.NewGuid()));
        await repo.AddAsync(new Entite("e2@example.com", "hash", "Entite 2", "Desc 2", "0600000002", Guid.NewGuid()));

        var résultats = await repo.GetAllAsync();

        Assert.Equal(2, résultats.Count);
    }

    [Fact]
    public async Task UpdateAsync_Desactiver_EstActivePersisté()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new EntiteRepository(ctx);

        var entite = new Entite("asso2@example.com", "hash", "Médecins Sans Frontières", "MSF", "0700000000", Guid.NewGuid());
        await repo.AddAsync(entite);

        entite.Desactiver();
        await repo.UpdateAsync(entite);

        var chargée = await repo.GetByIdAsync(entite.Id);
        Assert.False(chargée!.EstActive);
    }
}
