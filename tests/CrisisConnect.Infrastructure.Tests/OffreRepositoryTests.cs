using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class OffreRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetByIdAsync_RetourneOffre()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new OffreRepository(ctx);

        var offre = new Offre("Titre test", "Description test", Guid.NewGuid());
        await repo.AddAsync(offre);

        var résultat = await repo.GetByIdAsync(offre.Id);

        Assert.NotNull(résultat);
        Assert.Equal("Titre test", résultat.Titre);
        Assert.Equal(StatutProposition.Active, résultat.Statut);
    }

    [Fact]
    public async Task GetAllAsync_PlusieursOffres_RetourneTous()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new OffreRepository(ctx);

        await repo.AddAsync(new Offre("Offre 1", "Desc 1", Guid.NewGuid()));
        await repo.AddAsync(new Offre("Offre 2", "Desc 2", Guid.NewGuid()));
        await repo.AddAsync(new Offre("Offre 3", "Desc 3", Guid.NewGuid()));

        var résultats = await repo.GetAllAsync();

        Assert.Equal(3, résultats.Count);
    }

    [Fact]
    public async Task GetByIdAsync_IdInexistant_RetourneNull()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new OffreRepository(ctx);

        var résultat = await repo.GetByIdAsync(Guid.NewGuid());

        Assert.Null(résultat);
    }

    [Fact]
    public async Task UpdateAsync_StatutModifié_PersistéEnBase()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new OffreRepository(ctx);

        var offre = new Offre("Titre", "Desc", Guid.NewGuid());
        await repo.AddAsync(offre);

        offre.MarquerEnTransaction();
        await repo.UpdateAsync(offre);

        var chargée = await repo.GetByIdAsync(offre.Id);
        Assert.Equal(StatutProposition.EnTransaction, chargée!.Statut);
    }
}
