using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.GetDemandesQuota;

public record GetDemandesQuotaQuery : IRequest<List<DemandeQuotaDto>>;
