using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Tests;

public class LigneCatalogueTests
{
    private static LigneCatalogue Créer() =>
        new(Guid.NewGuid(), "REF-001", "Table scolaire", 20, 45.0, "https://catalogue.example.com/table");

    [Fact]
    public void NouvelleInstance_StatutEstDemandee()
    {
        var l = Créer();
        Assert.Equal(StatutLigne.Demandee, l.Statut);
        Assert.Equal("REF-001", l.Reference);
        Assert.Equal(20, l.Quantite);
    }

    [Fact]
    public void MarquerPartiellementFournie_ChangéStatut()
    {
        var l = Créer();
        l.MarquerPartiellementFournie();
        Assert.Equal(StatutLigne.PartiellementFournie, l.Statut);
    }

    [Fact]
    public void MarquerFournie_ChangéStatut()
    {
        var l = Créer();
        l.MarquerFournie();
        Assert.Equal(StatutLigne.Fournie, l.Statut);
    }
}
