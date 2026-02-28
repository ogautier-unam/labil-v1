using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;

namespace CrisisConnect.Domain.Tests;

public class PanierTests
{
    private static Panier CréerPanier() => new(Guid.NewGuid());

    private static Offre CréerOffre() => new("Titre", "Desc", Guid.NewGuid());

    [Fact]
    public void Panier_Création_StatutInitialEstOuvert()
    {
        var panier = CréerPanier();

        Assert.Equal(StatutPanier.Ouvert, panier.Statut);
        Assert.Empty(panier.Offres);
        Assert.NotEqual(Guid.Empty, panier.Id);
    }

    [Fact]
    public void Panier_AjouterOffre_OffreApparaîtDansLaCollection()
    {
        var panier = CréerPanier();
        var offre = CréerOffre();

        panier.AjouterOffre(offre);

        Assert.Single(panier.Offres);
        Assert.Contains(offre, panier.Offres);
    }

    [Fact]
    public void Panier_AjouterMêmeOffreDeuXFois_LèveUneException()
    {
        var panier = CréerPanier();
        var offre = CréerOffre();
        panier.AjouterOffre(offre);

        Assert.Throws<DomainException>(() => panier.AjouterOffre(offre));
    }

    [Fact]
    public void Panier_AjouterOffreSurPanierConfirmé_LèveUneException()
    {
        var panier = CréerPanier();
        panier.Confirmer();

        Assert.Throws<DomainException>(() => panier.AjouterOffre(CréerOffre()));
    }

    [Fact]
    public void Panier_AjouterOffreSurPanierAnnulé_LèveUneException()
    {
        var panier = CréerPanier();
        panier.Annuler();

        Assert.Throws<DomainException>(() => panier.AjouterOffre(CréerOffre()));
    }

    [Fact]
    public void Panier_Confirmer_ChangeLStatutEnConfirme()
    {
        var panier = CréerPanier();

        panier.Confirmer();

        Assert.Equal(StatutPanier.Confirme, panier.Statut);
        Assert.NotNull(panier.DateConfirmation);
    }

    [Fact]
    public void Panier_ConfirmerUnPanierDéjàConfirmé_LèveUneException()
    {
        var panier = CréerPanier();
        panier.Confirmer();

        Assert.Throws<DomainException>(() => panier.Confirmer());
    }

    [Fact]
    public void Panier_ConfirmerUnPanierAnnulé_LèveUneException()
    {
        var panier = CréerPanier();
        panier.Annuler();

        Assert.Throws<DomainException>(() => panier.Confirmer());
    }

    [Fact]
    public void Panier_Annuler_ChangeLStatutEnAnnule()
    {
        var panier = CréerPanier();

        panier.Annuler();

        Assert.Equal(StatutPanier.Annule, panier.Statut);
    }

    [Fact]
    public void Panier_AnnulerDeuxFois_LèveUneException()
    {
        var panier = CréerPanier();
        panier.Annuler();

        Assert.Throws<DomainException>(() => panier.Annuler());
    }
}
