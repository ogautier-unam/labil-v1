using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class AttributionRoleRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetByIdAsync_RetourneAttribution()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new AttributionRoleRepository(ctx);

        var acteurId = Guid.NewGuid();
        var attribution = new AttributionRole(acteurId, TypeRole.Contributeur, DateTime.Today);
        await repo.AddAsync(attribution);

        var résultat = await repo.GetByIdAsync(attribution.Id);

        Assert.NotNull(résultat);
        Assert.Equal(acteurId, résultat.ActeurId);
        Assert.Equal(TypeRole.Contributeur, résultat.TypeRole);
        Assert.Equal(StatutRole.Actif, résultat.Statut);
    }

    [Fact]
    public async Task GetByActeurAsync_DeuxAttributions_RetourneLesDeux()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new AttributionRoleRepository(ctx);

        var acteurId = Guid.NewGuid();
        await repo.AddAsync(new AttributionRole(acteurId, TypeRole.Contributeur, DateTime.Today));
        await repo.AddAsync(new AttributionRole(acteurId, TypeRole.AdminCatastrophe, DateTime.Today));

        var résultats = await repo.GetByActeurAsync(acteurId);

        Assert.Equal(2, résultats.Count);
        Assert.All(résultats, r => Assert.Equal(acteurId, r.ActeurId));
    }

    [Fact]
    public async Task GetByTypeRoleAsync_RôleActif_RetourneSeulement()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new AttributionRoleRepository(ctx);

        var actif = new AttributionRole(Guid.NewGuid(), TypeRole.AdminInstalleur, DateTime.Today);
        var expire = new AttributionRole(Guid.NewGuid(), TypeRole.AdminInstalleur, DateTime.Today);
        await repo.AddAsync(actif);
        await repo.AddAsync(expire);

        expire.Expirer();
        await repo.UpdateAsync(expire);

        var résultats = await repo.GetByTypeRoleAsync(TypeRole.AdminInstalleur);

        Assert.Single(résultats);
        Assert.Equal(actif.Id, résultats[0].Id);
    }

    [Fact]
    public async Task UpdateAsync_Expirer_StatutPersisté()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new AttributionRoleRepository(ctx);

        var attribution = new AttributionRole(Guid.NewGuid(), TypeRole.Contributeur, DateTime.Today);
        await repo.AddAsync(attribution);

        attribution.Expirer();
        await repo.UpdateAsync(attribution);

        var chargée = await repo.GetByIdAsync(attribution.Id);
        Assert.Equal(StatutRole.Expire, chargée!.Statut);
    }
}
