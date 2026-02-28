using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.ValueObjects;

namespace CrisisConnect.Domain.Entities;

/// <summary>
/// Proposition nécessitant la validation d'un tiers de confiance avant visibilité.
/// §5.1.3 : aide psychologique, aide à l'enfance — éviter les abus sur personnes vulnérables.
/// Le tiers = Entité reconnue par l'AC.
/// </summary>
public class PropositionAvecValidation : Proposition
{
    public string DescriptionValidation { get; private set; } = string.Empty;
    public StatutValidation StatutValidation { get; private set; } = StatutValidation.EnAttente;
    public Guid? ValideurEntiteId { get; private set; }

    protected PropositionAvecValidation() { }

    public PropositionAvecValidation(string titre, string description, Guid creePar,
        string descriptionValidation, Localisation? localisation = null)
    {
        Titre = titre;
        Description = description;
        CreePar = creePar;
        DescriptionValidation = descriptionValidation;
        Localisation = localisation;
        // Non visible tant que non validée
        Statut = StatutProposition.Archivee;
    }

    public void Valider(Guid valideurEntiteId)
    {
        if (StatutValidation != StatutValidation.EnAttente)
            throw new DomainException("La proposition est déjà validée ou refusée.");
        StatutValidation = StatutValidation.Validee;
        ValideurEntiteId = valideurEntiteId;
        Statut = StatutProposition.Active;
    }

    public void RefuserValidation(Guid valideurEntiteId)
    {
        StatutValidation = StatutValidation.Refusee;
        ValideurEntiteId = valideurEntiteId;
    }

    public override void Clore()
    {
        if (Statut == StatutProposition.Cloturee)
            throw new DomainException("La proposition est déjà clôturée.");
        Statut = StatutProposition.Cloturee;
        DateCloture = DateTime.UtcNow;
        ModifieLe = DateTime.UtcNow;
    }
}
