using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.ValueObjects;
using Mediator;

namespace CrisisConnect.Application.UseCases.Demandes.UpdateDemande;

public class UpdateDemandeCommandHandler : IRequestHandler<UpdateDemandeCommand, DemandeDto>
{
    private readonly IDemandeRepository _repository;
    private readonly AppMapper _mapper;

    public UpdateDemandeCommandHandler(IDemandeRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<DemandeDto> Handle(UpdateDemandeCommand request, CancellationToken cancellationToken)
    {
        var demande = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Demande), request.Id);

        Localisation? localisation = null;
        if (request.Latitude.HasValue && request.Longitude.HasValue)
            localisation = new Localisation(request.Latitude.Value, request.Longitude.Value);

        demande.Modifier(request.Titre, request.Description, request.Urgence, request.RegionSeverite, localisation);
        demande.ConfigurerRecurrence(request.EstRecurrente, request.FrequenceRecurrence);
        await _repository.UpdateAsync(demande, cancellationToken);

        return _mapper.ToDto(demande);
    }
}
