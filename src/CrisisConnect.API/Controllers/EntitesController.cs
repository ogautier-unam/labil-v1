using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Entites.CreateEntite;
using CrisisConnect.Application.UseCases.Entites.DesactiverEntite;
using CrisisConnect.Application.UseCases.Entites.GetEntites;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/entites")]
public class EntitesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EntitesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Liste toutes les entités (organisations).</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<EntiteDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEntitesQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Crée une nouvelle entité organisationnelle (Responsable uniquement).</summary>
    [HttpPost]
    [Authorize(Roles = "Responsable")]
    [ProducesResponseType<EntiteDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEntiteCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAll), result);
    }

    /// <summary>Désactive une entité (Responsable uniquement).</summary>
    [HttpPatch("{id:guid}/desactiver")]
    [Authorize(Roles = "Responsable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desactiver(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DesactiverEntiteCommand(id), cancellationToken);
        return NoContent();
    }
}
