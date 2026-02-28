using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Propositions.GetPropositionById;

public class GetPropositionByIdQueryHandler : IRequestHandler<GetPropositionByIdQuery, PropositionDto>
{
    private readonly IPropositionRepository _repository;
    private readonly IMapper _mapper;

    public GetPropositionByIdQueryHandler(IPropositionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PropositionDto> Handle(GetPropositionByIdQuery request, CancellationToken cancellationToken)
    {
        var proposition = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Proposition), request.Id);

        return _mapper.Map<PropositionDto>(proposition);
    }
}
