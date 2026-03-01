using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Services;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>Stratégie par type — groupe les demandes par catégorie taxonomique.</summary>
public class PriorisationParType : IStrategiePriorisation
{
#pragma warning disable S2325 // interface implementation — cannot be static
    public string Nom => "type";
#pragma warning restore S2325

    public IReadOnlyList<Demande> Trier(IEnumerable<Demande> demandes)
        => demandes.OrderBy(d => d.CategorieId)
                   .ThenByDescending(d => d.Urgence)
                   .ToList();
}
