using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Services;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>Stratégie par type — groupe les demandes par catégorie taxonomique.</summary>
public class PriorisationParType : IStrategiePriorisation
{
    public IReadOnlyList<Demande> Trier(IEnumerable<Demande> demandes)
        => demandes.OrderBy(d => d.CategorieId)
                   .ThenByDescending(d => d.Urgence)
                   .ToList();
}
