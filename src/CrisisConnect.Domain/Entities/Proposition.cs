using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.ValueObjects;

namespace CrisisConnect.Domain.Entities;

public class Proposition
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Titre { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public StatutProposition Statut { get; private set; } = StatutProposition.Ouverte;
    public Localisation? Localisation { get; private set; }
    public Guid CreePar { get; private set; }
    public DateTime CreeLe { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifieLe { get; private set; }

    protected Proposition() { }

    public Proposition(string titre, string description, Guid creePar, Localisation? localisation = null)
    {
        Titre = titre;
        Description = description;
        CreePar = creePar;
        Localisation = localisation;
    }

    public void Affecter()
    {
        if (Statut != StatutProposition.Ouverte)
            throw new DomainException("Seule une proposition ouverte peut être affectée.");

        Statut = StatutProposition.Affectee;
        ModifieLe = DateTime.UtcNow;
    }

    public void Cloturer()
    {
        if (Statut == StatutProposition.Cloturee)
            throw new DomainException("La proposition est déjà clôturée.");

        Statut = StatutProposition.Cloturee;
        ModifieLe = DateTime.UtcNow;
    }
}
