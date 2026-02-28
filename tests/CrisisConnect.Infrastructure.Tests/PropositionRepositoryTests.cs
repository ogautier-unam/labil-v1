using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class PropositionRepositoryTests
{
    [Fact]
    public async Task AddAsync_Offre_PuisGetByIdAsync_RetourneProposition()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new PropositionRepository(ctx);

        var offre = new Offre("Véhicule disponible", "Camion 3.5t", Guid.NewGuid());
        await repo.AddAsync(offre);

        var résultat = await repo.GetByIdAsync(offre.Id);

        Assert.NotNull(résultat);
        Assert.IsType<Offre>(résultat);
        Assert.Equal("Véhicule disponible", résultat.Titre);
    }

    [Fact]
    public async Task GetAllAsync_OffreEtDemandeMixtes_RetourneLesDeux()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new PropositionRepository(ctx);

        await repo.AddAsync(new Offre("Offre eau", "Eau potable", Guid.NewGuid()));
        await repo.AddAsync(new Demande("Demande médicaments", "Anti-douleurs urgents", Guid.NewGuid()));

        var résultats = await repo.GetAllAsync();

        Assert.Equal(2, résultats.Count);
        Assert.Contains(résultats, p => p is Offre);
        Assert.Contains(résultats, p => p is Demande);
    }

    [Fact]
    public async Task GetByIdAsync_IdInexistant_RetourneNull()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new PropositionRepository(ctx);

        var résultat = await repo.GetByIdAsync(Guid.NewGuid());

        Assert.Null(résultat);
    }

    [Fact]
    public async Task UpdateAsync_ChangerStatut_PersistéEnBase()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new PropositionRepository(ctx);

        var offre = new Offre("Hébergement", "Appartement disponible", Guid.NewGuid());
        await repo.AddAsync(offre);

        offre.MarquerEnAttenteRelance();
        await repo.UpdateAsync(offre);

        var chargée = await repo.GetByIdAsync(offre.Id);
        Assert.Equal(StatutProposition.EnAttenteRelance, chargée!.Statut);
    }

    [Fact]
    public async Task DeleteAsync_PropositionExistante_PlusRetournéeParGetAll()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new PropositionRepository(ctx);

        var offre = new Offre("À supprimer", "Test suppression", Guid.NewGuid());
        await repo.AddAsync(offre);

        await repo.DeleteAsync(offre);

        var résultats = await repo.GetAllAsync();
        Assert.Empty(résultats);
    }
}
