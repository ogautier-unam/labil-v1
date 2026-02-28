using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Infrastructure.Persistence;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class TransactionRepositoryTests
{
    private static async Task<(Offre offre, Transaction transaction)> CréerTransactionAsync(
        TransactionRepository repo, AppDbContext ctx)
    {
        var offre = new Offre("Titre", "Desc", Guid.NewGuid());
        await ctx.Propositions.AddAsync(offre);
        await ctx.SaveChangesAsync();

        var tx = new Transaction(offre.Id, Guid.NewGuid());
        await repo.AddAsync(tx);
        return (offre, tx);
    }

    [Fact]
    public async Task AddAsync_PuisGetByIdAsync_RetourneTransactionAvecDiscussion()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new TransactionRepository(ctx);

        var (_, tx) = await CréerTransactionAsync(repo, ctx);

        var résultat = await repo.GetByIdAsync(tx.Id);

        Assert.NotNull(résultat);
        Assert.Equal(StatutTransaction.EnCours, résultat.Statut);
        Assert.NotNull(résultat.Discussion);
    }

    [Fact]
    public async Task GetAllAsync_DeuxTransactions_RetourneLesDeuxAvecDiscussion()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new TransactionRepository(ctx);

        await CréerTransactionAsync(repo, ctx);
        await CréerTransactionAsync(repo, ctx);

        var résultats = await repo.GetAllAsync();

        Assert.Equal(2, résultats.Count);
        Assert.All(résultats, t => Assert.NotNull(t.Discussion));
    }

    [Fact]
    public async Task UpdateAsync_StatutConfirmé_PersistéEnBase()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new TransactionRepository(ctx);

        var (_, tx) = await CréerTransactionAsync(repo, ctx);

        tx.Confirmer();
        await repo.UpdateAsync(tx);

        var chargée = await repo.GetByIdAsync(tx.Id);
        Assert.Equal(StatutTransaction.Confirmee, chargée!.Statut);
        Assert.NotNull(chargée.DateMaj);
    }

    [Fact]
    public async Task GetByPropositionIdAsync_FiltreCorrectement()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new TransactionRepository(ctx);

        var (offre1, tx1) = await CréerTransactionAsync(repo, ctx);
        await CréerTransactionAsync(repo, ctx); // autre transaction

        var résultats = await repo.GetByPropositionIdAsync(offre1.Id);

        Assert.Single(résultats);
        Assert.Equal(tx1.Id, résultats[0].Id);
    }
}
