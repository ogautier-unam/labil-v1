using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;

namespace CrisisConnect.Domain.Tests;

public class OffreTests
{
    private static Offre CréerOffre(bool livraisonIncluse = false)
        => new("Titre test", "Description test", Guid.NewGuid(), livraisonIncluse);

    [Fact]
    public void Offre_CréationValide_StatutInitialEstActive()
    {
        var offre = CréerOffre();

        Assert.Equal(StatutProposition.Active, offre.Statut);
        Assert.Equal("Titre test", offre.Titre);
        Assert.Equal("Description test", offre.Description);
        Assert.NotEqual(Guid.Empty, offre.Id);
    }

    [Fact]
    public void Offre_LivraisonIncluse_EstConservé()
    {
        var offre = CréerOffre(livraisonIncluse: true);
        Assert.True(offre.LivraisonIncluse);
    }

    [Fact]
    public void Offre_LivraisonNonIncluse_ParDéfaut()
    {
        var offre = CréerOffre();
        Assert.False(offre.LivraisonIncluse);
    }

    [Fact]
    public void Offre_Clore_ChangeLStatutEnCloturee()
    {
        var offre = CréerOffre();

        offre.Clore();

        Assert.Equal(StatutProposition.Cloturee, offre.Statut);
        Assert.NotNull(offre.DateCloture);
    }

    [Fact]
    public void Offre_CloreDeuxFois_LèveUneException()
    {
        var offre = CréerOffre();
        offre.Clore();

        Assert.Throws<DomainException>(() => offre.Clore());
    }

    [Fact]
    public void Offre_Archiver_ChangeLStatutEnArchivee()
    {
        var offre = CréerOffre();

        offre.Archiver();

        Assert.Equal(StatutProposition.Archivee, offre.Statut);
        Assert.NotNull(offre.DateArchivage);
    }

    [Fact]
    public void Offre_ArchiverUneOffreClôturée_LèveUneException()
    {
        var offre = CréerOffre();
        offre.Clore();

        Assert.Throws<DomainException>(() => offre.Archiver());
    }

    [Fact]
    public void Offre_MarquerEnTransaction_ChangeLStatutEnEnTransaction()
    {
        var offre = CréerOffre();

        offre.MarquerEnTransaction();

        Assert.Equal(StatutProposition.EnTransaction, offre.Statut);
    }

    [Fact]
    public void Offre_MarquerEnTransactionSiNonActive_LèveUneException()
    {
        var offre = CréerOffre();
        offre.Clore();

        Assert.Throws<DomainException>(() => offre.MarquerEnTransaction());
    }

    [Fact]
    public void Offre_LibererDeTransaction_RemetLStatutEnActive()
    {
        var offre = CréerOffre();
        offre.MarquerEnTransaction();

        offre.LibererDeTransaction();

        Assert.Equal(StatutProposition.Active, offre.Statut);
    }
}
