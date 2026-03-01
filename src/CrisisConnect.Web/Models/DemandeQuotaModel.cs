namespace CrisisConnect.Web.Models;

public record DemandeQuotaModel(
    Guid Id,
    string Titre,
    string Description,
    string Statut,
    Guid CreePar,
    DateTime CreeLe,
    int CapaciteMax,
    string UniteCapacite,
    int CapaciteUtilisee,
    string? AdresseDepot,
    DateTime? DateLimit,
    List<IntentionDonModel> Intentions);
