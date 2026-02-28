using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Tests;

public class CategorieTaxonomieTests
{
    private static CategorieTaxonomie CréerCategorie(Guid? parentId = null) =>
        new("LOGEMENT", "{\"fr\":\"Logement\"}", Guid.NewGuid(), parentId);

    [Fact]
    public void CategorieTaxonomie_NouvelleInstance_EstActiveEtSansSousCategories()
    {
        var cat = CréerCategorie();

        Assert.True(cat.EstActive);
        Assert.Empty(cat.SousCategories);
        Assert.Equal("LOGEMENT", cat.Code);
    }

    [Fact]
    public void CategorieTaxonomie_AvecParent_ParentIdRenseigné()
    {
        var parentId = Guid.NewGuid();

        var cat = CréerCategorie(parentId);

        Assert.Equal(parentId, cat.ParentId);
    }

    [Fact]
    public void Desactiver_CategorieActive_EstActiveFaux()
    {
        var cat = CréerCategorie();

        cat.Desactiver();

        Assert.False(cat.EstActive);
    }

    [Fact]
    public void Reactiver_CategorieDesactivée_EstActiveVrai()
    {
        var cat = CréerCategorie();
        cat.Desactiver();

        cat.Reactiver();

        Assert.True(cat.EstActive);
    }
}
