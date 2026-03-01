using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.ValueObjects;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.CreateDemandeQuota;

public class CreateDemandeQuotaCommandHandler : IRequestHandler<CreateDemandeQuotaCommand, DemandeQuotaDto>
{
    private readonly IDemandeQuotaRepository _repository;

    public CreateDemandeQuotaCommandHandler(IDemandeQuotaRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<DemandeQuotaDto> Handle(CreateDemandeQuotaCommand request, CancellationToken cancellationToken)
    {
        Localisation? localisation = null;
        if (request.Latitude.HasValue && request.Longitude.HasValue)
            localisation = new Localisation(request.Latitude.Value, request.Longitude.Value);

        var demande = new DemandeQuota(
            request.Titre, request.Description, request.CreePar,
            request.CapaciteMax, request.UniteCapacite,
            request.AdresseDepot, request.DateLimit, localisation);

        await _repository.AddAsync(demande, cancellationToken);

        return AppMapper.ToDto(demande);
    }
}
