using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.ValueObjects;

namespace CrisisConnect.Domain.Entities;

public class Demande : Proposition
{
    public OperateurLogique OperateurLogique { get; private set; } = OperateurLogique.Simple;
    public NiveauUrgence Urgence { get; private set; } = NiveauUrgence.Moyen;
    public string? RegionSeverite { get; private set; }
    public Guid? ParentId { get; private set; }

    private readonly List<Demande> _sousDemandes = [];
    public IReadOnlyCollection<Demande> SousDemandes => _sousDemandes.AsReadOnly();

    protected Demande() { }

    public Demande(string titre, string description, Guid creePar,
        OperateurLogique operateur = OperateurLogique.Simple,
        NiveauUrgence urgence = NiveauUrgence.Moyen,
        Localisation? localisation = null,
        string? regionSeverite = null)
    {
        Titre = titre;
        Description = description;
        CreePar = creePar;
        OperateurLogique = operateur;
        Urgence = urgence;
        Localisation = localisation;
        RegionSeverite = regionSeverite;
    }

    public void Modifier(string titre, string description, NiveauUrgence urgence, string? regionSeverite, Localisation? localisation)
    {
        ModifierContenu(titre, description, localisation);
        Urgence = urgence;
        RegionSeverite = regionSeverite;
    }

    public override void Clore()
    {
        if (Statut == StatutProposition.Cloturee)
            throw new DomainException("La demande est déjà clôturée.");
        Statut = StatutProposition.Cloturee;
        DateCloture = DateTime.UtcNow;
        ModifieLe = DateTime.UtcNow;
        // Clôture récursive (Pattern Composite) — même comportement ET et OU depuis le parent
        foreach (var sousDemande in _sousDemandes)
            if (sousDemande.Statut != StatutProposition.Cloturee)
                sousDemande.Clore();
    }

    /// <summary>
    /// Propagation OU ascendante : quand une sous-demande est satisfaite dans un groupe OU,
    /// ferme toutes les alternatives sœurs non encore clôturées (§5.1.2 ex.3).
    /// À appeler sur le PARENT après avoir clôturé une de ses sous-demandes.
    /// </summary>
    public void ClorerAlternativesOu(Guid sousDemandeSatisfaiteId)
    {
        if (OperateurLogique != OperateurLogique.Ou)
            return;
        foreach (var soeur in _sousDemandes)
            if (soeur.Id != sousDemandeSatisfaiteId && soeur.Statut != StatutProposition.Cloturee)
                soeur.Clore();
    }
}
