using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.ValueObjects;
using MediatR;

namespace CrisisConnect.Application.UseCases.Propositions.CreateProposition;

public class CreatePropositionCommandHandler : IRequestHandler<CreatePropositionCommand, PropositionDto>
{
    private readonly IPropositionRepository _repository;
    private readonly IMapper _mapper;

    public CreatePropositionCommandHandler(IPropositionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PropositionDto> Handle(CreatePropositionCommand request, CancellationToken cancellationToken)
    {
        Localisation? localisation = null;
        if (request.Latitude.HasValue && request.Longitude.HasValue)
            localisation = new Localisation(request.Latitude.Value, request.Longitude.Value);

        var proposition = new Proposition(request.Titre, request.Description, request.CreePar, localisation);
        await _repository.AddAsync(proposition, cancellationToken);

        return _mapper.Map<PropositionDto>(proposition);
    }
}
