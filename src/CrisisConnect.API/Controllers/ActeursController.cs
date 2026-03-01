using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Acteurs.GetActeur;
using CrisisConnect.Application.UseCases.Acteurs.UpdateActeur;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/acteurs")]
[Authorize]
public class ActeursController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActeursController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Retourne le profil public d'un acteur (§5 ex.12).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<PersonneDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActeurQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Met à jour le profil d'un acteur (§5 ex.12). Seul l'acteur lui-même peut modifier son profil.</summary>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType<PersonneDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateActeurCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);
        return Ok(result);
    }
}
