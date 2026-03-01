using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.SoumettreIntentionDon;

public record SoumettreIntentionDonCommand(
    Guid DemandeQuotaId,
    Guid ActeurId,
    int Quantite,
    string Unite,
    string Description) : IRequest<IntentionDonDto>;
