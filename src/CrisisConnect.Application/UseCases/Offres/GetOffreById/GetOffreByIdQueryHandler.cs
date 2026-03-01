using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Offres.GetOffreById;

public class GetOffreByIdQueryHandler : IRequestHandler<GetOffreByIdQuery, OffreDto>
{
    private readonly IOffreRepository _repository;

    public GetOffreByIdQueryHandler(IOffreRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<OffreDto> Handle(GetOffreByIdQuery request, CancellationToken cancellationToken)
    {
        var offre = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Offre), request.Id);
        return AppMapper.ToDto(offre);
    }
}
