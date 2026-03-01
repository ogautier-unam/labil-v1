using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record IntentionDonDto(
    Guid Id,
    Guid DemandeQuotaId,
    Guid ActeurId,
    int Quantite,
    string Unite,
    string Description,
    DateTime DateIntention,
    StatutIntention Statut);
