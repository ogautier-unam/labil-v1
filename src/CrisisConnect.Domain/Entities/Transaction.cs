using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;

namespace CrisisConnect.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid PropositionId { get; private set; }
    public Guid InitiateurId { get; private set; }
    public StatutTransaction Statut { get; private set; } = StatutTransaction.EnCours;
    public DateTime DateCreation { get; private set; } = DateTime.UtcNow;
    public DateTime? DateMaj { get; private set; }

    public Discussion Discussion { get; private set; } = null!;

    protected Transaction() { }

    public Transaction(Guid propositionId, Guid initiateurId)
    {
        PropositionId = propositionId;
        InitiateurId = initiateurId;
        Discussion = new Discussion();
    }

    public void Confirmer()
    {
        if (Statut != StatutTransaction.EnCours)
            throw new DomainException("Seule une transaction en cours peut être confirmée.");
        Statut = StatutTransaction.Confirmee;
        DateMaj = DateTime.UtcNow;
    }

    public void Annuler()
    {
        if (Statut == StatutTransaction.Annulee)
            throw new DomainException("La transaction est déjà annulée.");
        Statut = StatutTransaction.Annulee;
        DateMaj = DateTime.UtcNow;
    }
}
