using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class MandatRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetByIdAsync_RetourneMandat()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new MandatRepository(ctx);

        var mandantId = Guid.NewGuid();
        var mandataireId = Guid.NewGuid();
        var mandat = new Mandat(mandantId, mandataireId, PorteeMandat.LectureSeule, "Délégation test", false, DateTime.Today);
        await repo.AddAsync(mandat);

        var résultat = await repo.GetByIdAsync(mandat.Id);

        Assert.NotNull(résultat);
        Assert.Equal(mandantId, résultat.MandantId);
        Assert.Equal(mandataireId, résultat.MandataireId);
        Assert.Equal("Délégation test", résultat.Description);
        Assert.True(résultat.EstActif);
    }

    [Fact]
    public async Task GetByMandantAsync_DeuxMandats_RetourneLesDeux()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new MandatRepository(ctx);

        var mandantId = Guid.NewGuid();
        await repo.AddAsync(new Mandat(mandantId, Guid.NewGuid(), PorteeMandat.LectureSeule, "Mandat A", false, DateTime.Today));
        await repo.AddAsync(new Mandat(mandantId, Guid.NewGuid(), PorteeMandat.ToutesOperations, "Mandat B", true, DateTime.Today));

        var résultats = await repo.GetByMandantAsync(mandantId);

        Assert.Equal(2, résultats.Count);
        Assert.All(résultats, m => Assert.Equal(mandantId, m.MandantId));
    }

    [Fact]
    public async Task GetByMandataireAsync_UnMandat_RetourneLeMandat()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new MandatRepository(ctx);

        var mandataireId = Guid.NewGuid();
        var mandat = new Mandat(Guid.NewGuid(), mandataireId, PorteeMandat.CategorieSpecifique, "Délégation catégorie", false, DateTime.Today);
        await repo.AddAsync(mandat);

        var résultats = await repo.GetByMandataireAsync(mandataireId);

        Assert.Single(résultats);
        Assert.Equal(mandataireId, résultats[0].MandataireId);
    }

    [Fact]
    public async Task UpdateAsync_Revoquer_EstActifFalse()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new MandatRepository(ctx);

        var mandat = new Mandat(Guid.NewGuid(), Guid.NewGuid(), PorteeMandat.ToutesOperations, "À révoquer", false, DateTime.Today);
        await repo.AddAsync(mandat);

        mandat.Revoquer();
        await repo.UpdateAsync(mandat);

        var chargé = await repo.GetByIdAsync(mandat.Id);
        Assert.False(chargé!.EstActif);
    }
}
