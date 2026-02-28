using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;

namespace CrisisConnect.Domain.Tests;

/// <summary>
/// Tests des méthodes communes de la classe abstraite Proposition (via Offre).
/// </summary>
public class PropositionTests
{
    private static Offre NouvelleOffre() =>
        new("Titre test", "Description test", Guid.NewGuid());

    [Fact]
    public void AjouterMedia_AjouteMediaAvecBonType()
    {
        var offre = NouvelleOffre();

        offre.AjouterMedia("https://cdn.example.com/photo.jpg", TypeMedia.Photo);

        Assert.Single(offre.Medias);
        Assert.Equal(TypeMedia.Photo, offre.Medias.First().Type);
    }

    [Fact]
    public void Archiver_StatutActif_ChangéEnArchivee()
    {
        var offre = NouvelleOffre();

        offre.Archiver();

        Assert.Equal(StatutProposition.Archivee, offre.Statut);
        Assert.NotNull(offre.DateArchivage);
    }

    [Fact]
    public void Archiver_StatutCloturee_LèveDomainException()
    {
        var offre = NouvelleOffre();
        offre.Clore();

        Assert.Throws<DomainException>(() => offre.Archiver());
    }

    [Fact]
    public void MarquerEnAttenteRelance_StatutActif_ChangéEtDateRenseignée()
    {
        var offre = NouvelleOffre();

        offre.MarquerEnAttenteRelance();

        Assert.Equal(StatutProposition.EnAttenteRelance, offre.Statut);
        Assert.NotNull(offre.DateRelance);
    }

    [Fact]
    public void MarquerEnAttenteRelance_StatutNonActif_LèveDomainException()
    {
        var offre = NouvelleOffre();
        offre.Archiver();

        Assert.Throws<DomainException>(() => offre.MarquerEnAttenteRelance());
    }

    [Fact]
    public void Reconfirmer_StatutEnAttenteRelance_RedevientActif()
    {
        var offre = NouvelleOffre();
        offre.MarquerEnAttenteRelance();

        offre.Reconfirmer();

        Assert.Equal(StatutProposition.Active, offre.Statut);
    }

    [Fact]
    public void MarquerEnTransaction_StatutActif_ChangéEnTransaction()
    {
        var offre = NouvelleOffre();

        offre.MarquerEnTransaction();

        Assert.Equal(StatutProposition.EnTransaction, offre.Statut);
    }

    [Fact]
    public void LibererDeTransaction_StatutEnTransaction_RedevientActif()
    {
        var offre = NouvelleOffre();
        offre.MarquerEnTransaction();

        offre.LibererDeTransaction();

        Assert.Equal(StatutProposition.Active, offre.Statut);
    }
}
