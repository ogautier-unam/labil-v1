using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.RefuserIntentionDon;

public record RefuserIntentionDonCommand(Guid DemandeQuotaId, Guid IntentionId) : IRequest<DemandeQuotaDto>;
