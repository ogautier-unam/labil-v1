using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.ValueObjects;

namespace CrisisConnect.Domain.Entities;

public class Offre : Proposition
{
    public bool LivraisonIncluse { get; private set; }

    protected Offre() { }

    public Offre(string titre, string description, Guid creePar, bool livraisonIncluse = false, Localisation? localisation = null)
    {
        Titre = titre;
        Description = description;
        CreePar = creePar;
        LivraisonIncluse = livraisonIncluse;
        Localisation = localisation;
    }

    public override void Clore()
    {
        if (Statut == StatutProposition.Cloturee)
            throw new DomainException("L'offre est déjà clôturée.");
        Statut = StatutProposition.Cloturee;
        DateCloture = DateTime.UtcNow;
        ModifieLe = DateTime.UtcNow;
    }
}
