using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.ConfirmerIntentionDon;

public record ConfirmerIntentionDonCommand(Guid DemandeQuotaId, Guid IntentionId) : IRequest<DemandeQuotaDto>;
