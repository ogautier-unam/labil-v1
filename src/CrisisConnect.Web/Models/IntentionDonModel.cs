namespace CrisisConnect.Web.Models;

public record IntentionDonModel(
    Guid Id,
    Guid DemandeQuotaId,
    Guid ActeurId,
    int Quantite,
    string Unite,
    string Description,
    DateTime DateIntention,
    string Statut);
