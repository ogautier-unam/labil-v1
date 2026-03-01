using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeSurCatalogue.CreateDemandeSurCatalogue;

public class CreateDemandeSurCatalogueHandler : IRequestHandler<CreateDemandeSurCatalogueCommand, DemandeSurCatalogueDto>
{
    private readonly IDemandeSurCatalogueRepository _repository;

    public CreateDemandeSurCatalogueHandler(IDemandeSurCatalogueRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<DemandeSurCatalogueDto> Handle(CreateDemandeSurCatalogueCommand request, CancellationToken cancellationToken)
    {
        var demande = new Domain.Entities.DemandeSurCatalogue(
            request.Titre,
            request.Description,
            request.CreePar,
            request.UrlCatalogue);

        await _repository.AddAsync(demande, cancellationToken);
        return AppMapper.ToDto(demande);
    }
}
