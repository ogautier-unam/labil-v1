using CrisisConnect.Domain.Entities;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class RefreshTokenRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetByTokenAsync_RetourneToken()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new RefreshTokenRepository(ctx);

        var personneId = Guid.NewGuid();
        var token = new RefreshToken("tok-abc123", personneId, DateTime.UtcNow.AddDays(7));
        await repo.AddAsync(token);

        var résultat = await repo.GetByTokenAsync("tok-abc123");

        Assert.NotNull(résultat);
        Assert.Equal(personneId, résultat.PersonneId);
        Assert.True(résultat.EstValide);
    }

    [Fact]
    public async Task GetByTokenAsync_TokenInexistant_RetourneNull()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new RefreshTokenRepository(ctx);

        var résultat = await repo.GetByTokenAsync("introuvable");

        Assert.Null(résultat);
    }

    [Fact]
    public async Task UpdateAsync_Revoquer_EstRévoquéPersisté()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new RefreshTokenRepository(ctx);

        var token = new RefreshToken("tok-révocable", Guid.NewGuid(), DateTime.UtcNow.AddDays(7));
        await repo.AddAsync(token);

        token.Revoquer();
        await repo.UpdateAsync(token);

        var chargé = await repo.GetByTokenAsync("tok-révocable");
        Assert.True(chargé!.EstRevoque);
    }

    [Fact]
    public async Task RevoquerTousAsync_DeuxTokens_TousRévoqués()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new RefreshTokenRepository(ctx);

        var personneId = Guid.NewGuid();
        await repo.AddAsync(new RefreshToken("tok-1", personneId, DateTime.UtcNow.AddDays(7)));
        await repo.AddAsync(new RefreshToken("tok-2", personneId, DateTime.UtcNow.AddDays(7)));
        await repo.AddAsync(new RefreshToken("tok-autre", Guid.NewGuid(), DateTime.UtcNow.AddDays(7)));

        await repo.RevoquerTousAsync(personneId);

        var tok1 = await repo.GetByTokenAsync("tok-1");
        var tok2 = await repo.GetByTokenAsync("tok-2");
        var tokAutre = await repo.GetByTokenAsync("tok-autre");
        Assert.True(tok1!.EstRevoque);
        Assert.True(tok2!.EstRevoque);
        Assert.False(tokAutre!.EstRevoque);
    }
}
