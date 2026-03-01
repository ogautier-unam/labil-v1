using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeRepartitionGeo.GetDemandesRepartitionGeo;

public class GetDemandesRepartitionGeoHandler : IRequestHandler<GetDemandesRepartitionGeoQuery, List<DemandeRepartitionGeoDto>>
{
    private readonly IDemandeRepartitionGeoRepository _repository;

    public GetDemandesRepartitionGeoHandler(IDemandeRepartitionGeoRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<List<DemandeRepartitionGeoDto>> Handle(GetDemandesRepartitionGeoQuery request, CancellationToken cancellationToken)
    {
        var demandes = await _repository.GetAllAsync(cancellationToken);
        return demandes.Select(AppMapper.ToDto).ToList();
    }
}
