using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeSurCatalogue.GetDemandeSurCatalogueById;

public class GetDemandeSurCatalogueByIdHandler : IRequestHandler<GetDemandeSurCatalogueByIdQuery, DemandeSurCatalogueDto?>
{
    private readonly IDemandeSurCatalogueRepository _repository;

    public GetDemandeSurCatalogueByIdHandler(IDemandeSurCatalogueRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<DemandeSurCatalogueDto?> Handle(GetDemandeSurCatalogueByIdQuery request, CancellationToken cancellationToken)
    {
        var demande = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return demande is null ? null : AppMapper.ToDto(demande);
    }
}
