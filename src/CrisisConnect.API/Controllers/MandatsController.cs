using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Mandats.CreerMandat;
using CrisisConnect.Application.UseCases.Mandats.GetMandats;
using CrisisConnect.Application.UseCases.Mandats.RevoquerMandat;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/mandats")]
[Authorize]
public class MandatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MandatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Retourne les mandats émis par un acteur.</summary>
    [HttpGet("mandant/{acteurId:guid}")]
    [ProducesResponseType<IReadOnlyList<MandatDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMandant(Guid acteurId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMandatsQuery(acteurId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Crée un mandat de délégation.</summary>
    [HttpPost]
    [ProducesResponseType<MandatDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Creer([FromBody] CreerMandatCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetByMandant), new { acteurId = result.MandantId }, result);
    }

    /// <summary>Révoque un mandat (Responsable uniquement).</summary>
    [HttpPatch("{id:guid}/revoquer")]
    [Authorize(Roles = "Responsable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Revoquer(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RevoquerMandatCommand(id), cancellationToken);
        return NoContent();
    }
}
