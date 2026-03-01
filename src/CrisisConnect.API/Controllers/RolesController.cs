using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Roles.AttribuerRole;
using CrisisConnect.Application.UseCases.Roles.GetRolesActeur;
using CrisisConnect.Application.UseCases.Roles.RevoquerRole;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = "Coordinateur,Responsable")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Retourne les rôles d'un acteur.</summary>
    [HttpGet("acteur/{acteurId:guid}")]
    [ProducesResponseType<IReadOnlyList<AttributionRoleDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByActeur(Guid acteurId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRolesActeurQuery(acteurId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Attribue un rôle à un acteur (Coordinateur/Responsable uniquement).</summary>
    [HttpPost]
    [ProducesResponseType<AttributionRoleDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Attribuer([FromBody] AttribuerRoleCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetByActeur), new { acteurId = result.ActeurId }, result);
    }

    /// <summary>Révoque un rôle (marque l'attribution comme expirée).</summary>
    [HttpPatch("{id:guid}/revoquer")]
    [Authorize(Roles = "Responsable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Revoquer(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RevoquerRoleCommand(id), cancellationToken);
        return NoContent();
    }
}
