using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Suggestions.AcknowledgeSuggestion;
using CrisisConnect.Application.UseCases.Suggestions.GenererSuggestions;
using CrisisConnect.Application.UseCases.Suggestions.GetNonAcknowledgedSuggestions;
using CrisisConnect.Application.UseCases.Suggestions.GetSuggestionsByDemande;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/suggestions")]
[Authorize]
public class SuggestionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuggestionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Retourne les suggestions d'appariement pour une demande donnée.</summary>
    [HttpGet("demande/{demandeId:guid}")]
    [ProducesResponseType<IReadOnlyList<SuggestionAppariementDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDemande(Guid demandeId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSuggestionsByDemandeQuery(demandeId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Retourne toutes les suggestions non encore acquittées (Coordinateur).</summary>
    [HttpGet("pending")]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType<IReadOnlyList<SuggestionAppariementDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetNonAcknowledgedSuggestionsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Génère automatiquement des suggestions d'appariement pour une demande active.
    /// Seul le Coordinateur ou le Responsable peut déclencher la génération.
    /// </summary>
    [HttpPost("demande/{demandeId:guid}/generer")]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType<IReadOnlyList<SuggestionAppariementDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Generer(Guid demandeId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GenererSuggestionsCommand(demandeId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Acquitte une suggestion (marque comme traitée).</summary>
    [HttpPatch("{id:guid}/acknowledge")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Acknowledge(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AcknowledgeSuggestionCommand(id), cancellationToken);
        return NoContent();
    }
}
