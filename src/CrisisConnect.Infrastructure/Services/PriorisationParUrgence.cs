using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Services;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>Stratégie urgence — CRITIQUE > ELEVE > MOYEN > FAIBLE.</summary>
public class PriorisationParUrgence : IStrategiePriorisation
{
#pragma warning disable S2325 // interface implementation — cannot be static
    public string Nom => "urgence";
#pragma warning restore S2325

    public IReadOnlyList<Demande> Trier(IEnumerable<Demande> demandes)
        => demandes.OrderByDescending(d => d.Urgence).ToList();
}
