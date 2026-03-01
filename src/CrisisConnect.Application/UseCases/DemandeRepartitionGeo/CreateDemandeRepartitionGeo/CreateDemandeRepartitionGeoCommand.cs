using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandeRepartitionGeo.CreateDemandeRepartitionGeo;

public record CreateDemandeRepartitionGeoCommand(
    string Titre,
    string Description,
    Guid CreePar,
    int NombreRessourcesRequises,
    string DescriptionMission) : IRequest<DemandeRepartitionGeoDto>;
