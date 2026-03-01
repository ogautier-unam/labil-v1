using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using Mediator;

namespace CrisisConnect.Application.UseCases.Demandes.GetDemandes;

public class GetDemandesQueryHandler : IRequestHandler<GetDemandesQuery, IReadOnlyList<DemandeDto>>
{
    private readonly IDemandeRepository _repository;
    private readonly AppMapper _mapper;
    private readonly IEnumerable<IStrategiePriorisation> _strategies;

    public GetDemandesQueryHandler(
        IDemandeRepository repository,
        AppMapper mapper,
        IEnumerable<IStrategiePriorisation> strategies)
    {
        _repository = repository;
        _mapper = mapper;
        _strategies = strategies;
    }

    public async ValueTask<IReadOnlyList<DemandeDto>> Handle(GetDemandesQuery request, CancellationToken cancellationToken)
    {
        var demandes = await _repository.GetAllAsync(cancellationToken);

        if (request.Statut.HasValue)
            demandes = demandes.Where(d => d.Statut == request.Statut.Value).ToList();

        if (request.Urgence.HasValue)
            demandes = demandes.Where(d => d.Urgence == request.Urgence.Value).ToList();

        // NF-11 — appliquer la stratégie de priorisation si spécifiée
        if (request.Strategie is not null)
        {
            var strategie = _strategies.FirstOrDefault(s =>
                s.Nom.Equals(request.Strategie, StringComparison.OrdinalIgnoreCase));
            if (strategie is not null)
                demandes = strategie.Trier(demandes).ToList();
        }

        return _mapper.ToDto(demandes);
    }
}
