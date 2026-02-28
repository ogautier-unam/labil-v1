using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Services;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>Stratégie FIFO — demandes les plus anciennes en premier.</summary>
public class PriorisationParAnciennete : IStrategiePriorisation
{
    public IReadOnlyList<Demande> Trier(IEnumerable<Demande> demandes)
        => demandes.OrderBy(d => d.CreeLe).ToList();
}
