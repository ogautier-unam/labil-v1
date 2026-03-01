using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Demandes.GetDemandes;

public class GetDemandesQueryHandler : IRequestHandler<GetDemandesQuery, IReadOnlyList<DemandeDto>>
{
    private readonly IDemandeRepository _repository;
    private readonly AppMapper _mapper;

    public GetDemandesQueryHandler(IDemandeRepository repository, AppMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<IReadOnlyList<DemandeDto>> Handle(GetDemandesQuery request, CancellationToken cancellationToken)
    {
        var demandes = await _repository.GetAllAsync(cancellationToken);

        if (request.Statut.HasValue)
            demandes = demandes.Where(d => d.Statut == request.Statut.Value).ToList();

        if (request.Urgence.HasValue)
            demandes = demandes.Where(d => d.Urgence == request.Urgence.Value).ToList();

        return _mapper.ToDto(demandes);
    }
}
