using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeSurCatalogue.GetDemandeSurCatalogueById;

public record GetDemandeSurCatalogueByIdQuery(Guid Id) : IRequest<DemandeSurCatalogueDto?>;
