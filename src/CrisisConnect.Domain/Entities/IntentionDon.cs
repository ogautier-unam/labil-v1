using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;

namespace CrisisConnect.Domain.Entities;

/// <summary>
/// Intention de don soumise en réponse à une DemandeQuota.
/// §5.1.3 : portée à CONFIRMEE une fois le don effectivement déposé.
/// </summary>
public class IntentionDon
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid DemandeQuotaId { get; private set; }
    public Guid ActeurId { get; private set; }
    public int Quantite { get; private set; }
    public string Unite { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime DateIntention { get; private set; } = DateTime.UtcNow;
    public StatutIntention Statut { get; private set; } = StatutIntention.EnAttente;

    protected IntentionDon() { }

    public IntentionDon(Guid demandeQuotaId, Guid acteurId, int quantite, string unite, string description)
    {
        DemandeQuotaId = demandeQuotaId;
        ActeurId = acteurId;
        Quantite = quantite;
        Unite = unite;
        Description = description;
    }

    public void Accepter()
    {
        if (Statut != StatutIntention.EnAttente)
            throw new DomainException("L'intention n'est pas en attente.");
        Statut = StatutIntention.Acceptee;
    }

    public void Refuser()
    {
        if (Statut != StatutIntention.EnAttente)
            throw new DomainException("L'intention n'est pas en attente.");
        Statut = StatutIntention.Refusee;
    }

    public void Confirmer()
    {
        if (Statut != StatutIntention.Acceptee)
            throw new DomainException("L'intention doit être acceptée avant d'être confirmée.");
        Statut = StatutIntention.Confirmee;
    }
}
