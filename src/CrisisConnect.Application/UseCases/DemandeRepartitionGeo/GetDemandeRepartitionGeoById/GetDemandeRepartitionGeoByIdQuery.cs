using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeRepartitionGeo.GetDemandeRepartitionGeoById;

public record GetDemandeRepartitionGeoByIdQuery(Guid Id) : IRequest<DemandeRepartitionGeoDto?>;
