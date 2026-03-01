using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Services;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>Stratégie FIFO — demandes les plus anciennes en premier.</summary>
public class PriorisationParAnciennete : IStrategiePriorisation
{
#pragma warning disable S2325 // interface implementation — cannot be static
    public string Nom => "anciennete";
#pragma warning restore S2325

    public IReadOnlyList<Demande> Trier(IEnumerable<Demande> demandes)
        => demandes.OrderBy(d => d.CreeLe).ToList();
}
