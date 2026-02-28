using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Missions.AssignBenevole;

public record AssignBenevoleCommand(Guid MissionId, Guid BenevoleId) : IRequest<MatchingDto>;
