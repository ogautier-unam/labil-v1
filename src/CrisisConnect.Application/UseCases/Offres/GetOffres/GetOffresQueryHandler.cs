using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Offres.GetOffres;

public class GetOffresQueryHandler : IRequestHandler<GetOffresQuery, IReadOnlyList<OffreDto>>
{
    private readonly IOffreRepository _repository;

    public GetOffresQueryHandler(IOffreRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<IReadOnlyList<OffreDto>> Handle(GetOffresQuery request, CancellationToken cancellationToken)
    {
        var offres = await _repository.GetAllAsync(cancellationToken);

        if (request.Statut.HasValue)
            offres = offres.Where(o => o.Statut == request.Statut.Value).ToList();

        return AppMapper.ToDto(offres.ToList());
    }
}
