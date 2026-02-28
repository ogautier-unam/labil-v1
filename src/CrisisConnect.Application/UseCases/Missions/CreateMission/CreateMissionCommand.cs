using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Missions.CreateMission;

public record CreateMissionCommand(
    string Titre,
    string Description,
    Guid PropositionId,
    Guid CreePar,
    int NombreBenevoles,
    DateTime? DebutPlage = null,
    DateTime? FinPlage = null) : IRequest<MissionDto>;
