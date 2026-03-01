using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Enums;
using Mediator;

namespace CrisisConnect.Application.UseCases.Demandes.UpdateDemande;

public record UpdateDemandeCommand(
    Guid Id,
    string Titre,
    string Description,
    NiveauUrgence Urgence = NiveauUrgence.Moyen,
    string? RegionSeverite = null,
    double? Latitude = null,
    double? Longitude = null) : IRequest<DemandeDto>;
