using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class PanierRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetByIdAsync_RetournePanierAvecOffres()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new PanierRepository(ctx);

        var proprietaireId = Guid.NewGuid();
        var panier = new Panier(proprietaireId);
        await repo.AddAsync(panier);

        var résultat = await repo.GetByIdAsync(panier.Id);

        Assert.NotNull(résultat);
        Assert.Equal(proprietaireId, résultat.ProprietaireId);
        Assert.Equal(StatutPanier.Ouvert, résultat.Statut);
        Assert.Empty(résultat.Offres);
    }

    [Fact]
    public async Task GetByProprietaireAsync_RetourneSeulementLesPaniersDeceProprietaire()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new PanierRepository(ctx);

        var propId1 = Guid.NewGuid();
        var propId2 = Guid.NewGuid();
        await repo.AddAsync(new Panier(propId1));
        await repo.AddAsync(new Panier(propId1));
        await repo.AddAsync(new Panier(propId2));

        var résultats = await repo.GetByProprietaireAsync(propId1);

        Assert.Equal(2, résultats.Count);
        Assert.All(résultats, p => Assert.Equal(propId1, p.ProprietaireId));
    }

    [Fact]
    public async Task UpdateAsync_AjouterOffrePuisPersister_OffreChargéeAvecPanier()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new PanierRepository(ctx);

        // Persister l'offre d'abord (requise pour la relation many-to-many)
        var offre = new Offre("Titre offre", "Desc", Guid.NewGuid());
        await ctx.Propositions.AddAsync(offre);
        await ctx.SaveChangesAsync();

        var panier = new Panier(Guid.NewGuid());
        await repo.AddAsync(panier);

        panier.AjouterOffre(offre);
        await repo.UpdateAsync(panier);

        var chargé = await repo.GetByIdAsync(panier.Id);
        Assert.NotNull(chargé);
        Assert.Single(chargé.Offres);
        Assert.Equal(offre.Id, chargé.Offres.First().Id);
    }

    [Fact]
    public async Task UpdateAsync_ConfirmerPanier_StatutConfirméPersisté()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new PanierRepository(ctx);

        var panier = new Panier(Guid.NewGuid());
        await repo.AddAsync(panier);

        panier.Confirmer();
        await repo.UpdateAsync(panier);

        var chargé = await repo.GetByIdAsync(panier.Id);
        Assert.Equal(StatutPanier.Confirme, chargé!.Statut);
        Assert.NotNull(chargé.DateConfirmation);
    }
}
