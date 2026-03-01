using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeSurCatalogue.GetDemandeSurCatalogues;

public record GetDemandeSurCataloguesQuery : IRequest<List<DemandeSurCatalogueDto>>;
