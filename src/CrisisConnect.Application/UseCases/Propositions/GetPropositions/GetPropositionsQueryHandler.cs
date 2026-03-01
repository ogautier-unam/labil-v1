using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.GetPropositions;

public class GetPropositionsQueryHandler : IRequestHandler<GetPropositionsQuery, IReadOnlyList<PropositionDto>>
{
    private readonly IPropositionRepository _repository;
    private readonly AppMapper _mapper;

    public GetPropositionsQueryHandler(IPropositionRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<IReadOnlyList<PropositionDto>> Handle(GetPropositionsQuery request, CancellationToken cancellationToken)
    {
        var propositions = await _repository.GetAllAsync(cancellationToken);
        return _mapper.ToDto(propositions);
    }
}
