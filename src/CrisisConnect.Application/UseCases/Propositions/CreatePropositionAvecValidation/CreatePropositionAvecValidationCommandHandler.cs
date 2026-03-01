using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.ValueObjects;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.CreatePropositionAvecValidation;

public class CreatePropositionAvecValidationCommandHandler
    : IRequestHandler<CreatePropositionAvecValidationCommand, PropositionAvecValidationDto>
{
    private readonly IPropositionRepository _repository;
    private readonly AppMapper _mapper;

    public CreatePropositionAvecValidationCommandHandler(IPropositionRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<PropositionAvecValidationDto> Handle(
        CreatePropositionAvecValidationCommand request, CancellationToken cancellationToken)
    {
        Localisation? localisation = null;
        if (request.Latitude.HasValue && request.Longitude.HasValue)
            localisation = new Localisation(request.Latitude.Value, request.Longitude.Value);

        var proposition = new PropositionAvecValidation(
            request.Titre, request.Description, request.CreePar,
            request.DescriptionValidation, localisation);

        await _repository.AddAsync(proposition, cancellationToken);

        return _mapper.ToDto(proposition);
    }
}
