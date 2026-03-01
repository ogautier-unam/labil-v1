using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeSurCatalogue.AjouterLigneCatalogue;

public record AjouterLigneCatalogueCommand(
    Guid DemandeSurCatalogueId,
    string Reference,
    string Designation,
    int Quantite,
    double PrixUnitaire,
    string? UrlProduit) : IRequest<LigneCatalogueDto>;
