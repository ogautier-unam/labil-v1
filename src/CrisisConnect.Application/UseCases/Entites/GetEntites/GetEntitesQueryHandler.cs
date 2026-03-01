using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Entites.GetEntites;

public class GetEntitesQueryHandler : IRequestHandler<GetEntitesQuery, IReadOnlyList<EntiteDto>>
{
    private readonly IEntiteRepository _repository;
    private readonly IMapper _mapper;

    public GetEntitesQueryHandler(IEntiteRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EntiteDto>> Handle(GetEntitesQuery request, CancellationToken cancellationToken)
    {
        var entites = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<EntiteDto>>(entites);
    }
}
