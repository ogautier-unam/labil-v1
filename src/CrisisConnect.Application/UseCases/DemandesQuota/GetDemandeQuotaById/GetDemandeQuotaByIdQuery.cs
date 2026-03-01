using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.GetDemandeQuotaById;

public record GetDemandeQuotaByIdQuery(Guid Id) : IRequest<DemandeQuotaDto>;
