using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Services;

/// <summary>
/// Pattern Strategy (GoF) — stratégie de priorisation des demandes.
/// Implémentations : ancienneté, urgence, région/sévérité, type taxonomique.
/// §5.1.2 ex.1 : extensible par plugins sans modifier le code existant.
/// </summary>
public interface IStrategiePriorisation
{
    /// <summary>Identifiant de la stratégie (anciennete | urgence | region | type).</summary>
    string Nom { get; }

    IReadOnlyList<Demande> Trier(IEnumerable<Demande> demandes);
}
