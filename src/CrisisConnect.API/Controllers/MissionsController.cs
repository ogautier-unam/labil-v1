using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Missions.AssignBenevole;
using CrisisConnect.Application.UseCases.Missions.CreateMission;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Crée une nouvelle mission.</summary>
    [HttpPost]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType<MissionDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMissionCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }

    /// <summary>Assigne un bénévole à une mission (matching).</summary>
    [HttpPost("{missionId:guid}/matchings")]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType<MatchingDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignBenevole(Guid missionId, [FromBody] AssignBenevoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AssignBenevoleCommand(missionId, request.BenevoleId), cancellationToken);
        return CreatedAtAction(nameof(AssignBenevole), new { missionId, id = result.Id }, result);
    }
}

public record AssignBenevoleRequest(Guid BenevoleId);
