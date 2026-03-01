using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeRepartitionGeo.GetDemandesRepartitionGeo;

public record GetDemandesRepartitionGeoQuery : IRequest<List<DemandeRepartitionGeoDto>>;
