using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Services;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>Stratégie urgence — CRITIQUE > ELEVE > MOYEN > FAIBLE.</summary>
public class PriorisationParUrgence : IStrategiePriorisation
{
    public IReadOnlyList<Demande> Trier(IEnumerable<Demande> demandes)
        => demandes.OrderByDescending(d => d.Urgence).ToList();
}
