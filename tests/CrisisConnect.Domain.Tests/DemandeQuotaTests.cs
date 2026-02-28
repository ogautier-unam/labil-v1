using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;

namespace CrisisConnect.Domain.Tests;

public class DemandeQuotaTests
{
    private static DemandeQuota Créer(int capaciteMax = 100) =>
        new("Collecte eau", "Distribution eau potable", Guid.NewGuid(), capaciteMax, "m³");

    [Fact]
    public void AjouterIntention_SousCapacite_IntentionAjoutée()
    {
        var dq = Créer(100);

        var intention = dq.AjouterIntention(Guid.NewGuid(), 30, "m³", "Don partiel");

        Assert.Single(dq.Intentions);
        Assert.Equal(30, intention.Quantite);
        Assert.Equal(StatutIntention.EnAttente, intention.Statut);
    }

    [Fact]
    public void AjouterIntention_CapacitéDépassée_LèveDomainException()
    {
        var dq = Créer(50);

        Assert.Throws<DomainException>(() =>
            dq.AjouterIntention(Guid.NewGuid(), 60, "m³", "Trop grand"));
    }

    [Fact]
    public void ValiderIntention_IntentionExistante_CapaciteUtiliseeAugmentée()
    {
        var dq = Créer(100);
        var intention = dq.AjouterIntention(Guid.NewGuid(), 25, "m³", "Don");

        dq.ValiderIntention(intention.Id);

        Assert.Equal(StatutIntention.Acceptee, intention.Statut);
        Assert.Equal(25, dq.CapaciteUtilisee);
    }

    [Fact]
    public void ValiderIntention_IntentionInexistante_LèveNotFoundException()
    {
        var dq = Créer(100);

        Assert.Throws<NotFoundException>(() => dq.ValiderIntention(Guid.NewGuid()));
    }
}
