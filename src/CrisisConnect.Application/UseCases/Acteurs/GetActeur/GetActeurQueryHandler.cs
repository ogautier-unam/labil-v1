using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Acteurs.GetActeur;

public class GetActeurQueryHandler : IQueryHandler<GetActeurQuery, PersonneDto>
{
    private readonly IPersonneRepository _repository;

    public GetActeurQueryHandler(IPersonneRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<PersonneDto> Handle(GetActeurQuery request, CancellationToken cancellationToken)
    {
        var personne = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Personne), request.Id);

        return AppMapper.ToDto(personne);
    }
}
