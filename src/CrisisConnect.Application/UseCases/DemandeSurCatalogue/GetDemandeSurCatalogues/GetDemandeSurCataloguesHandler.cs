using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeSurCatalogue.GetDemandeSurCatalogues;

public class GetDemandeSurCataloguesHandler : IRequestHandler<GetDemandeSurCataloguesQuery, List<DemandeSurCatalogueDto>>
{
    private readonly IDemandeSurCatalogueRepository _repository;

    public GetDemandeSurCataloguesHandler(IDemandeSurCatalogueRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<List<DemandeSurCatalogueDto>> Handle(GetDemandeSurCataloguesQuery request, CancellationToken cancellationToken)
    {
        var demandes = await _repository.GetAllAsync(cancellationToken);
        return demandes.Select(AppMapper.ToDto).ToList();
    }
}
