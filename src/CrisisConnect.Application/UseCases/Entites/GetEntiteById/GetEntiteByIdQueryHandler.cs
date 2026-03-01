using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Entites.GetEntiteById;

public class GetEntiteByIdQueryHandler : IQueryHandler<GetEntiteByIdQuery, EntiteDto>
{
    private readonly IEntiteRepository _repository;
    private readonly AppMapper _mapper;

    public GetEntiteByIdQueryHandler(IEntiteRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<EntiteDto> Handle(GetEntiteByIdQuery request, CancellationToken cancellationToken)
    {
        var entite = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Entite), request.Id);

        return _mapper.ToDto(entite);
    }
}
