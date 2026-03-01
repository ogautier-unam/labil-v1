using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.AccepterIntentionDon;

public record AccepterIntentionDonCommand(Guid DemandeQuotaId, Guid IntentionId) : IRequest<DemandeQuotaDto>;
