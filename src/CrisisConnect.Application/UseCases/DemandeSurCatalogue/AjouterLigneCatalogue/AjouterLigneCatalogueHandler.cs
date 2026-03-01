using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.Mappings;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeSurCatalogue.AjouterLigneCatalogue;

public class AjouterLigneCatalogueHandler : IRequestHandler<AjouterLigneCatalogueCommand, LigneCatalogueDto>
{
    private readonly IDemandeSurCatalogueRepository _repository;

    public AjouterLigneCatalogueHandler(IDemandeSurCatalogueRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<LigneCatalogueDto> Handle(AjouterLigneCatalogueCommand request, CancellationToken cancellationToken)
    {
        var demande = await _repository.GetByIdAsync(request.DemandeSurCatalogueId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.DemandeSurCatalogue), request.DemandeSurCatalogueId);

        var ligne = demande.AjouterLigne(
            request.Reference,
            request.Designation,
            request.Quantite,
            request.PrixUnitaire,
            request.UrlProduit);

        await _repository.UpdateAsync(demande, cancellationToken);
        return AppMapper.ToDto(ligne);
    }
}
