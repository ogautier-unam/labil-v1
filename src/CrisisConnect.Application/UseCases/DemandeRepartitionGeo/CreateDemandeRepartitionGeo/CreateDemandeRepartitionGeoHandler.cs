using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeRepartitionGeo.CreateDemandeRepartitionGeo;

public class CreateDemandeRepartitionGeoHandler : IRequestHandler<CreateDemandeRepartitionGeoCommand, DemandeRepartitionGeoDto>
{
    private readonly IDemandeRepartitionGeoRepository _repository;

    public CreateDemandeRepartitionGeoHandler(IDemandeRepartitionGeoRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<DemandeRepartitionGeoDto> Handle(CreateDemandeRepartitionGeoCommand request, CancellationToken cancellationToken)
    {
        var demande = new Domain.Entities.DemandeRepartitionGeo(
            request.Titre,
            request.Description,
            request.CreePar,
            request.NombreRessourcesRequises,
            request.DescriptionMission);

        await _repository.AddAsync(demande, cancellationToken);
        return AppMapper.ToDto(demande);
    }
}
