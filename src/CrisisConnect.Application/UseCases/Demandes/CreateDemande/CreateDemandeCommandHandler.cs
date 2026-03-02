using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.ValueObjects;
using Mediator;

namespace CrisisConnect.Application.UseCases.Demandes.CreateDemande;

public class CreateDemandeCommandHandler : IRequestHandler<CreateDemandeCommand, DemandeDto>
{
    private readonly IDemandeRepository _repository;
    private readonly AppMapper _mapper;

    public CreateDemandeCommandHandler(IDemandeRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<DemandeDto> Handle(CreateDemandeCommand request, CancellationToken cancellationToken)
    {
        Localisation? localisation = null;
        if (request.Latitude.HasValue && request.Longitude.HasValue)
            localisation = new Localisation(request.Latitude.Value, request.Longitude.Value);

        var demande = new Demande(
            request.Titre,
            request.Description,
            request.CreePar,
            request.OperateurLogique,
            request.Urgence,
            localisation,
            request.RegionSeverite);

        if (request.EstRecurrente)
            demande.ConfigurerRecurrence(true, request.FrequenceRecurrence);

        await _repository.AddAsync(demande, cancellationToken);

        return _mapper.ToDto(demande);
    }
}
