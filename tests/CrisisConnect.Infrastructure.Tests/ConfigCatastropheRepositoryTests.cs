using CrisisConnect.Domain.Entities;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class ConfigCatastropheRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetActiveAsync_RetourneConfigActive()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new ConfigCatastropheRepository(ctx);

        var config = new ConfigCatastrophe("Inondation", "Crise inondation", "PACA", "France", 30, 7);
        await repo.AddAsync(config);

        var active = await repo.GetActiveAsync();

        Assert.NotNull(active);
        Assert.Equal("Inondation", active.Nom);
        Assert.True(active.EstActive);
    }

    [Fact]
    public async Task AddAsync_PuisGetByIdAsync_RetourneConfig()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new ConfigCatastropheRepository(ctx);

        var config = new ConfigCatastrophe("Séisme", "Crise sismique", "Méditerranée", "France");
        await repo.AddAsync(config);

        var résultat = await repo.GetByIdAsync(config.Id);

        Assert.NotNull(résultat);
        Assert.Equal(config.Id, résultat.Id);
        Assert.Equal("Séisme", résultat.Nom);
    }

    [Fact]
    public async Task UpdateAsync_Desactiver_PersistéEnBase()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new ConfigCatastropheRepository(ctx);

        var config = new ConfigCatastrophe("Pandémie", "Crise sanitaire", "France entière", "France");
        await repo.AddAsync(config);

        config.Desactiver();
        await repo.UpdateAsync(config);

        var chargée = await repo.GetByIdAsync(config.Id);
        Assert.False(chargée!.EstActive);

        var active = await repo.GetActiveAsync();
        Assert.Null(active);
    }
}
