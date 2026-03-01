using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.GetDemandesQuota;

public class GetDemandesQuotaQueryHandler : IRequestHandler<GetDemandesQuotaQuery, List<DemandeQuotaDto>>
{
    private readonly IDemandeQuotaRepository _repository;

    public GetDemandesQuotaQueryHandler(IDemandeQuotaRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<List<DemandeQuotaDto>> Handle(GetDemandesQuotaQuery request, CancellationToken cancellationToken)
    {
        var demandes = await _repository.GetAllAsync(cancellationToken);
        return demandes.Select(AppMapper.ToDto).ToList();
    }
}
