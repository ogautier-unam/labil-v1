using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;

namespace CrisisConnect.Domain.Tests;

public class TransactionTests
{
    private static Transaction CréerTransaction()
        => new(Guid.NewGuid(), Guid.NewGuid());

    [Fact]
    public void Transaction_Création_StatutInitialEstEnCours()
    {
        var tx = CréerTransaction();

        Assert.Equal(StatutTransaction.EnCours, tx.Statut);
        Assert.NotEqual(Guid.Empty, tx.Id);
        Assert.NotNull(tx.Discussion);
    }

    [Fact]
    public void Transaction_Confirmer_ChangeLStatutEnConfirmee()
    {
        var tx = CréerTransaction();

        tx.Confirmer();

        Assert.Equal(StatutTransaction.Confirmee, tx.Statut);
        Assert.NotNull(tx.DateMaj);
    }

    [Fact]
    public void Transaction_ConfirmerUneTransactionDéjàConfirmée_LèveUneException()
    {
        var tx = CréerTransaction();
        tx.Confirmer();

        Assert.Throws<DomainException>(() => tx.Confirmer());
    }

    [Fact]
    public void Transaction_ConfirmerUneTransactionAnnulée_LèveUneException()
    {
        var tx = CréerTransaction();
        tx.Annuler();

        Assert.Throws<DomainException>(() => tx.Confirmer());
    }

    [Fact]
    public void Transaction_Annuler_ChangeLStatutEnAnnulee()
    {
        var tx = CréerTransaction();

        tx.Annuler();

        Assert.Equal(StatutTransaction.Annulee, tx.Statut);
        Assert.NotNull(tx.DateMaj);
    }

    [Fact]
    public void Transaction_AnnulerDeuxFois_LèveUneException()
    {
        var tx = CréerTransaction();
        tx.Annuler();

        Assert.Throws<DomainException>(() => tx.Annuler());
    }
}
