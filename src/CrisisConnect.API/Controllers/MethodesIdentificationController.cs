using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.MethodesIdentification.GetMethodes;
using CrisisConnect.Application.UseCases.MethodesIdentification.VerifierMethode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/methodes-identification")]
[Authorize]
public class MethodesIdentificationController : ControllerBase
{
    private readonly IMediator _mediator;

    public MethodesIdentificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Retourne les méthodes d'identification d'une personne.</summary>
    [HttpGet("personne/{personneId:guid}")]
    [ProducesResponseType<IReadOnlyList<MethodeIdentificationDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPersonne(Guid personneId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMethodesQuery(personneId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Marque une méthode d'identification comme vérifiée (Coordinateur/Responsable).</summary>
    [HttpPatch("{id:guid}/verifier")]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Verifier(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new VerifierMethodeCommand(id), cancellationToken);
        return NoContent();
    }
}
