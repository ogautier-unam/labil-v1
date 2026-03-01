using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeSurCatalogue.CreateDemandeSurCatalogue;

public record CreateDemandeSurCatalogueCommand(
    string Titre,
    string Description,
    Guid CreePar,
    string UrlCatalogue) : IRequest<DemandeSurCatalogueDto>;
