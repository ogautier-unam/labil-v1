namespace CrisisConnect.Web.Models;

public record CreateDemandeQuotaRequest(
    string Titre,
    string Description,
    int CapaciteMax,
    string UniteCapacite,
    string? AdresseDepot = null,
    DateTime? DateLimit = null);
