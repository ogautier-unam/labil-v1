using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.GetPropositionById;

public class GetPropositionByIdQueryHandler : IRequestHandler<GetPropositionByIdQuery, PropositionDto>
{
    private readonly IPropositionRepository _repository;
    private readonly AppMapper _mapper;

    public GetPropositionByIdQueryHandler(IPropositionRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<PropositionDto> Handle(GetPropositionByIdQuery request, CancellationToken cancellationToken)
    {
        var proposition = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.Id);

        return _mapper.ToDto(proposition);
    }
}
