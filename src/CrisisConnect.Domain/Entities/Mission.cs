using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.ValueObjects;

namespace CrisisConnect.Domain.Entities;

public class Mission
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Titre { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public StatutMission Statut { get; private set; } = StatutMission.Planifiee;
    public Guid PropositionId { get; private set; }
    public Guid CreePar { get; private set; }
    public PlageTemporelle? Plage { get; private set; }
    public int NombreBenevoles { get; private set; }
    public DateTime CreeLe { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifieLe { get; private set; }

    private readonly List<Matching> _matchings = [];
    public IReadOnlyCollection<Matching> Matchings => _matchings.AsReadOnly();

    protected Mission() { }

    public Mission(string titre, string description, Guid propositionId, Guid creePar, int nombreBenevoles, PlageTemporelle? plage = null)
    {
        Titre = titre;
        Description = description;
        PropositionId = propositionId;
        CreePar = creePar;
        NombreBenevoles = nombreBenevoles;
        Plage = plage;
    }

    public void Demarrer()
    {
        if (Statut != StatutMission.Planifiee)
            throw new DomainException("Seule une mission planifiée peut être démarrée.");
        Statut = StatutMission.EnCours;
        ModifieLe = DateTime.UtcNow;
    }

    public void Terminer()
    {
        if (Statut != StatutMission.EnCours)
            throw new DomainException("Seule une mission en cours peut être terminée.");
        Statut = StatutMission.Terminee;
        ModifieLe = DateTime.UtcNow;
    }

    public void Annuler()
    {
        if (Statut == StatutMission.Terminee)
            throw new DomainException("Une mission terminée ne peut pas être annulée.");
        Statut = StatutMission.Annulee;
        ModifieLe = DateTime.UtcNow;
    }
}
