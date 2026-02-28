using CrisisConnect.Domain.Entities;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class CategorieTaxonomieRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetByIdAsync_RetourneCategorieAvecSousCategories()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new CategorieTaxonomieRepository(ctx);

        var configId = Guid.NewGuid();
        var parent = new CategorieTaxonomie("LOGEMENT", "{\"fr\":\"Logement\"}", configId);
        await repo.AddAsync(parent);

        var sousCategorie = new CategorieTaxonomie("HEBERGEMENT", "{\"fr\":\"Hébergement\"}", configId, parent.Id);
        await repo.AddAsync(sousCategorie);

        var résultat = await repo.GetByIdAsync(parent.Id);

        Assert.NotNull(résultat);
        Assert.Equal("LOGEMENT", résultat.Code);
        Assert.Single(résultat.SousCategories);
    }

    [Fact]
    public async Task GetRacinesAsync_FiltreSurConfigEtParentNull()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new CategorieTaxonomieRepository(ctx);

        var configId = Guid.NewGuid();
        var autreConfigId = Guid.NewGuid();

        var cat1 = new CategorieTaxonomie("CAT1", "{}", configId);
        var cat2 = new CategorieTaxonomie("CAT2", "{}", configId);
        var catAutreConfig = new CategorieTaxonomie("AUTRE", "{}", autreConfigId);
        await repo.AddAsync(cat1);
        await repo.AddAsync(cat2);
        await repo.AddAsync(catAutreConfig);

        // Sous-catégorie de cat1 : ne doit pas apparaître dans les racines
        var sousCat = new CategorieTaxonomie("SOUS", "{}", configId, cat1.Id);
        await repo.AddAsync(sousCat);

        var racines = await repo.GetRacinesAsync(configId);

        Assert.Equal(2, racines.Count);
        Assert.All(racines, c => Assert.Null(c.ParentId));
    }

    [Fact]
    public async Task GetSousCategoriesAsync_RetourneSeulementSousCategories()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new CategorieTaxonomieRepository(ctx);

        var configId = Guid.NewGuid();
        var parent = new CategorieTaxonomie("EVACUATION", "{}", configId);
        await repo.AddAsync(parent);

        var s1 = new CategorieTaxonomie("TRANSPORT", "{}", configId, parent.Id);
        var s2 = new CategorieTaxonomie("HEBERGEMENT_URGENCE", "{}", configId, parent.Id);
        await repo.AddAsync(s1);
        await repo.AddAsync(s2);

        var sousCategories = await repo.GetSousCategoriesAsync(parent.Id);

        Assert.Equal(2, sousCategories.Count);
        Assert.All(sousCategories, c => Assert.Equal(parent.Id, c.ParentId));
    }
}
