using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Services;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>Stratégie région/sévérité — zones géographiques les plus touchées en premier.</summary>
public class PriorisationParRegionSeverite : IStrategiePriorisation
{
#pragma warning disable S2325 // interface implementation — cannot be static
    public string Nom => "region";
#pragma warning restore S2325

    public IReadOnlyList<Demande> Trier(IEnumerable<Demande> demandes)
        => demandes.OrderBy(d => string.IsNullOrEmpty(d.RegionSeverite) ? 1 : 0)
                   .ThenBy(d => d.RegionSeverite)
                   .ToList();
}
