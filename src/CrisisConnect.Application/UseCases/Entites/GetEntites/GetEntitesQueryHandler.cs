using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Entites.GetEntites;

public class GetEntitesQueryHandler : IRequestHandler<GetEntitesQuery, IReadOnlyList<EntiteDto>>
{
    private readonly IEntiteRepository _repository;
    private readonly AppMapper _mapper;

    public GetEntitesQueryHandler(IEntiteRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<IReadOnlyList<EntiteDto>> Handle(GetEntitesQuery request, CancellationToken cancellationToken)
    {
        var entites = await _repository.GetAllAsync(cancellationToken);
        return _mapper.ToDto(entites);
    }
}
