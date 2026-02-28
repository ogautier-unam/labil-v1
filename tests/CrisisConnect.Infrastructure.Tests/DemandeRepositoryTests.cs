using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class DemandeRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetByIdAsync_RetourneDemande()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new DemandeRepository(ctx);

        var demande = new Demande("Besoin eau", "Urgence eau potable", Guid.NewGuid(),
            urgence: NiveauUrgence.Critique, regionSeverite: "Zone Nord");
        await repo.AddAsync(demande);

        var résultat = await repo.GetByIdAsync(demande.Id);

        Assert.NotNull(résultat);
        Assert.Equal("Besoin eau", résultat.Titre);
        Assert.Equal(NiveauUrgence.Critique, résultat.Urgence);
        Assert.Equal("Zone Nord", résultat.RegionSeverite);
        Assert.Equal(StatutProposition.Active, résultat.Statut);
    }

    [Fact]
    public async Task GetAllAsync_TroisDemandes_RetourneLesTrois()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new DemandeRepository(ctx);

        await repo.AddAsync(new Demande("D1", "Desc", Guid.NewGuid()));
        await repo.AddAsync(new Demande("D2", "Desc", Guid.NewGuid()));
        await repo.AddAsync(new Demande("D3", "Desc", Guid.NewGuid()));

        var résultats = await repo.GetAllAsync();

        Assert.Equal(3, résultats.Count);
    }

    [Fact]
    public async Task GetByIdAsync_IdInexistant_RetourneNull()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new DemandeRepository(ctx);

        var résultat = await repo.GetByIdAsync(Guid.NewGuid());

        Assert.Null(résultat);
    }

    [Fact]
    public async Task UpdateAsync_StatutModifié_PersistéEnBase()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new DemandeRepository(ctx);

        var demande = new Demande("Titre", "Desc", Guid.NewGuid());
        await repo.AddAsync(demande);

        demande.MarquerEnTransaction();
        await repo.UpdateAsync(demande);

        var chargée = await repo.GetByIdAsync(demande.Id);
        Assert.Equal(StatutProposition.EnTransaction, chargée!.Statut);
    }
}
