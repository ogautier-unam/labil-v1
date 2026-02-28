using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.ValueObjects;

namespace CrisisConnect.Domain.Entities;

/// <summary>
/// Demande collective avec quota de capacité (ex. 6 camions de 90m³).
/// §5.1.3 : fédérer des acteurs autour d'une aide logistique.
/// Enregistrement d'intentions de don + validation.
/// </summary>
public class DemandeQuota : Demande
{
    public int CapaciteMax { get; private set; }
    public string UniteCapacite { get; private set; } = string.Empty;
    public string? AdresseDepot { get; private set; }
    public DateTime? DateLimit { get; private set; }
    public int CapaciteUtilisee { get; private set; }

    private readonly List<IntentionDon> _intentions = [];
    public IReadOnlyCollection<IntentionDon> Intentions => _intentions.AsReadOnly();

    protected DemandeQuota() { }

    public DemandeQuota(string titre, string description, Guid creePar,
        int capaciteMax, string uniteCapacite, string? adresseDepot = null, DateTime? dateLimit = null,
        Localisation? localisation = null)
        : base(titre, description, creePar, localisation: localisation)
    {
        CapaciteMax = capaciteMax;
        UniteCapacite = uniteCapacite;
        AdresseDepot = adresseDepot;
        DateLimit = dateLimit;
    }

    public IntentionDon AjouterIntention(Guid acteurId, int quantite, string unite, string description)
    {
        if (CapaciteUtilisee + quantite > CapaciteMax)
            throw new DomainException("La capacité maximale serait dépassée.");

        var intention = new IntentionDon(Id, acteurId, quantite, unite, description);
        _intentions.Add(intention);
        return intention;
    }

    public void ValiderIntention(Guid intentionId)
    {
        var intention = _intentions.FirstOrDefault(i => i.Id == intentionId)
            ?? throw new NotFoundException(nameof(IntentionDon), intentionId);
        intention.Accepter();
        CapaciteUtilisee += intention.Quantite;
    }
}
