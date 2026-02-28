using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Propositions.GetPropositions;

public class GetPropositionsQueryHandler : IRequestHandler<GetPropositionsQuery, IReadOnlyList<PropositionDto>>
{
    private readonly IPropositionRepository _repository;
    private readonly IMapper _mapper;

    public GetPropositionsQueryHandler(IPropositionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PropositionDto>> Handle(GetPropositionsQuery request, CancellationToken cancellationToken)
    {
        var propositions = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<PropositionDto>>(propositions);
    }
}
