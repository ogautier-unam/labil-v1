using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;

namespace CrisisConnect.Domain.Entities;

public class Matching
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid MissionId { get; private set; }
    public Guid BenevoleId { get; private set; }
    public StatutMatching Statut { get; private set; } = StatutMatching.EnAttente;
    public DateTime CreeLe { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifieLe { get; private set; }

    protected Matching() { }

    public Matching(Guid missionId, Guid benevoleId)
    {
        MissionId = missionId;
        BenevoleId = benevoleId;
    }

    public void Accepter()
    {
        if (Statut != StatutMatching.EnAttente)
            throw new DomainException("Seul un matching en attente peut être accepté.");
        Statut = StatutMatching.Accepte;
        ModifieLe = DateTime.UtcNow;
    }

    public void Refuser()
    {
        if (Statut != StatutMatching.EnAttente)
            throw new DomainException("Seul un matching en attente peut être refusé.");
        Statut = StatutMatching.Refuse;
        ModifieLe = DateTime.UtcNow;
    }
}
