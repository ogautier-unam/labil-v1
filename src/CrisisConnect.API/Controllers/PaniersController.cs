using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Paniers.AjouterOffreAuPanier;
using CrisisConnect.Application.UseCases.Paniers.AnnulerPanier;
using CrisisConnect.Application.UseCases.Paniers.ConfirmerPanier;
using CrisisConnect.Application.UseCases.Paniers.CreatePanier;
using CrisisConnect.Application.UseCases.Paniers.GetPanier;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/paniers")]
[Authorize]
public class PaniersController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaniersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Retourne le panier ouvert d'un propriétaire.</summary>
    [HttpGet]
    [ProducesResponseType<PanierDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMien([FromQuery] Guid proprietaireId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPanierQuery(proprietaireId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Crée un panier pour un propriétaire.</summary>
    [HttpPost]
    [ProducesResponseType<PanierDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePanierCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetMien), new { proprietaireId = result.ProprietaireId }, result);
    }

    /// <summary>Ajoute une offre au panier.</summary>
    [HttpPost("{id:guid}/offres")]
    [ProducesResponseType<PanierDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AjouterOffre(Guid id, [FromBody] AjouterOffreRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AjouterOffreAuPanierCommand(id, request.OffreId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Confirme le panier (le clôture).</summary>
    [HttpPatch("{id:guid}/confirmer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Confirmer(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ConfirmerPanierCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>Annule le panier.</summary>
    [HttpPatch("{id:guid}/annuler")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Annuler(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AnnulerPanierCommand(id), cancellationToken);
        return NoContent();
    }
}

public record AjouterOffreRequest(Guid OffreId);
