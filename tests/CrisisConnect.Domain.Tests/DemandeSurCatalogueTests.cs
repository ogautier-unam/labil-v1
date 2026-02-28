using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Tests;

public class DemandeSurCatalogueTests
{
    [Fact]
    public void AjouterLigne_NouvelleInstance_LigneAjoutéeAvecStatutDemandee()
    {
        var dsc = new DemandeSurCatalogue("École Clairval", "Matériel scolaire",
            Guid.NewGuid(), "https://catalogue.example.com");

        var ligne = dsc.AjouterLigne("CRAIE-01", "Craies couleur", 10, 2.5);

        Assert.Single(dsc.Lignes);
        Assert.Equal("CRAIE-01", ligne.Reference);
        Assert.Equal(StatutLigne.Demandee, ligne.Statut);
    }

    [Fact]
    public void AjouterLigne_MultiplesFois_ToutesLignesPresentes()
    {
        var dsc = new DemandeSurCatalogue("École Clairval", "Matériel scolaire",
            Guid.NewGuid(), "https://catalogue.example.com");

        dsc.AjouterLigne("CRAIE-01", "Craies", 10, 2.5);
        dsc.AjouterLigne("TABLE-02", "Table scolaire", 5, 45.0);
        dsc.AjouterLigne("ORDI-03", "Ordinateur portable", 3, 399.0);

        Assert.Equal(3, dsc.Lignes.Count);
    }

    [Fact]
    public void AjouterLigne_LigneAvecUrlProduit_UrlSetée()
    {
        var dsc = new DemandeSurCatalogue("Test", "Description", Guid.NewGuid(), "https://cat.example.com");

        var ligne = dsc.AjouterLigne("REF-X", "Produit X", 1, 9.99, "https://cat.example.com/produit-x");

        Assert.Equal("https://cat.example.com/produit-x", ligne.UrlProduit);
    }
}
