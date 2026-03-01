using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeRepartitionGeo.GetDemandeRepartitionGeoById;

public class GetDemandeRepartitionGeoByIdHandler : IRequestHandler<GetDemandeRepartitionGeoByIdQuery, DemandeRepartitionGeoDto?>
{
    private readonly IDemandeRepartitionGeoRepository _repository;

    public GetDemandeRepartitionGeoByIdHandler(IDemandeRepartitionGeoRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<DemandeRepartitionGeoDto?> Handle(GetDemandeRepartitionGeoByIdQuery request, CancellationToken cancellationToken)
    {
        var demande = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return demande is null ? null : AppMapper.ToDto(demande);
    }
}
