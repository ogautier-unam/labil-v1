using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.ValueObjects;
using MediatR;

namespace CrisisConnect.Application.UseCases.Demandes.CreateDemande;

public class CreateDemandeCommandHandler : IRequestHandler<CreateDemandeCommand, DemandeDto>
{
    private readonly IDemandeRepository _repository;
    private readonly IMapper _mapper;

    public CreateDemandeCommandHandler(IDemandeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DemandeDto> Handle(CreateDemandeCommand request, CancellationToken cancellationToken)
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

        await _repository.AddAsync(demande, cancellationToken);

        return _mapper.Map<DemandeDto>(demande);
    }
}
