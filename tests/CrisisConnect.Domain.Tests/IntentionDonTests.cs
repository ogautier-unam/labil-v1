using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;

namespace CrisisConnect.Domain.Tests;

public class IntentionDonTests
{
    private static IntentionDon Créer() =>
        new(Guid.NewGuid(), Guid.NewGuid(), 10, "m³", "Eau potable");

    [Fact]
    public void NouvelleInstance_StatutEstEnAttente()
    {
        var i = Créer();
        Assert.Equal(StatutIntention.EnAttente, i.Statut);
    }

    [Fact]
    public void Accepter_StatutEnAttente_ChangéEnAcceptee()
    {
        var i = Créer();
        i.Accepter();
        Assert.Equal(StatutIntention.Acceptee, i.Statut);
    }

    [Fact]
    public void Accepter_StatutNonEnAttente_LèveDomainException()
    {
        var i = Créer();
        i.Refuser();
        Assert.Throws<DomainException>(() => i.Accepter());
    }

    [Fact]
    public void Refuser_StatutEnAttente_ChangéEnRefusee()
    {
        var i = Créer();
        i.Refuser();
        Assert.Equal(StatutIntention.Refusee, i.Statut);
    }

    [Fact]
    public void Confirmer_StatutAcceptee_ChangéEnConfirmee()
    {
        var i = Créer();
        i.Accepter();
        i.Confirmer();
        Assert.Equal(StatutIntention.Confirmee, i.Statut);
    }

    [Fact]
    public void Confirmer_StatutNonAcceptee_LèveDomainException()
    {
        var i = Créer();
        // EnAttente → Confirmer doit échouer
        Assert.Throws<DomainException>(() => i.Confirmer());
    }
}
