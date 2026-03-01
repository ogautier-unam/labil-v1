using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.DemandesQuota.CreateDemandeQuota;

public record CreateDemandeQuotaCommand(
    string Titre,
    string Description,
    Guid CreePar,
    int CapaciteMax,
    string UniteCapacite,
    string? AdresseDepot = null,
    DateTime? DateLimit = null,
    double? Latitude = null,
    double? Longitude = null) : IRequest<DemandeQuotaDto>;
