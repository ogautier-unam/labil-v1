using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record MissionDto(
    Guid Id,
    string Titre,
    string Description,
    StatutMission Statut,
    Guid PropositionId,
    Guid CreePar,
    int NombreBenevoles,
    DateTime CreeLe);

public record MatchingDto(
    Guid Id,
    Guid MissionId,
    Guid BenevoleId,
    StatutMatching Statut,
    DateTime CreeLe);
