using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;

namespace CrisisConnect.Domain.Tests;

public class DemandeTests
{
    private static Demande CréerDemande(NiveauUrgence urgence = NiveauUrgence.Moyen)
        => new("Titre demande", "Description demande", Guid.NewGuid(), urgence: urgence);

    [Fact]
    public void Demande_CréationValide_ProprietésInitialesCorrects()
    {
        var demande = CréerDemande();

        Assert.Equal(StatutProposition.Active, demande.Statut);
        Assert.Equal(NiveauUrgence.Moyen, demande.Urgence);
        Assert.Equal(OperateurLogique.Simple, demande.OperateurLogique);
        Assert.Empty(demande.SousDemandes);
    }

    [Fact]
    public void Demande_CréationAvecUrgenceCritique_UrgenceConservée()
    {
        var demande = CréerDemande(NiveauUrgence.Critique);
        Assert.Equal(NiveauUrgence.Critique, demande.Urgence);
    }

    [Fact]
    public void Demande_Clore_ChangeLStatutEnCloturee()
    {
        var demande = CréerDemande();

        demande.Clore();

        Assert.Equal(StatutProposition.Cloturee, demande.Statut);
        Assert.NotNull(demande.DateCloture);
    }

    [Fact]
    public void Demande_CloreDeuxFois_LèveUneException()
    {
        var demande = CréerDemande();
        demande.Clore();

        Assert.Throws<DomainException>(() => demande.Clore());
    }

    [Fact]
    public void Demande_MarquerEnAttenteRelance_ChangeLStatut()
    {
        var demande = CréerDemande();

        demande.MarquerEnAttenteRelance();

        Assert.Equal(StatutProposition.EnAttenteRelance, demande.Statut);
    }

    [Fact]
    public void Demande_Reconfirmer_RemetEnActive()
    {
        var demande = CréerDemande();
        demande.MarquerEnAttenteRelance();

        demande.Reconfirmer();

        Assert.Equal(StatutProposition.Active, demande.Statut);
    }

    [Fact]
    public void Demande_ReconfirmerSansAttenteRelance_LèveUneException()
    {
        var demande = CréerDemande();

        Assert.Throws<DomainException>(() => demande.Reconfirmer());
    }
}
