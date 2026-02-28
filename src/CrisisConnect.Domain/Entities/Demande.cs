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

    public override void Clore()
    {
        if (Statut == StatutProposition.Cloturee)
            throw new DomainException("La demande est déjà clôturée.");
        Statut = StatutProposition.Cloturee;
        DateCloture = DateTime.UtcNow;
        ModifieLe = DateTime.UtcNow;
        // Clôture récursive (Pattern Composite)
        foreach (var sousDemande in _sousDemandes)
            if (sousDemande.Statut != StatutProposition.Cloturee)
                sousDemande.Clore();
    }
}
