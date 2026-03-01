using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.DemandesQuota.AccepterIntentionDon;
using CrisisConnect.Application.UseCases.DemandesQuota.ConfirmerIntentionDon;
using CrisisConnect.Application.UseCases.DemandesQuota.CreateDemandeQuota;
using CrisisConnect.Application.UseCases.DemandesQuota.GetDemandeQuotaById;
using CrisisConnect.Application.UseCases.DemandesQuota.GetDemandesQuota;
using CrisisConnect.Application.UseCases.DemandesQuota.RefuserIntentionDon;
using CrisisConnect.Application.UseCases.DemandesQuota.SoumettreIntentionDon;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/demandes-quota")]
public class DemandesQuotaController : ControllerBase
{
    private readonly IMediator _mediator;

    public DemandesQuotaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Liste toutes les demandes quota.</summary>
    [HttpGet]
    [ProducesResponseType<List<DemandeQuotaDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDemandesQuotaQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Retourne une demande quota par son identifiant.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<DemandeQuotaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDemandeQuotaByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Crée une demande quota de capacité.</summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType<DemandeQuotaDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateDemandeQuotaCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Soumet une intention de don sur une demande quota.</summary>
    [HttpPost("{id:guid}/intentions")]
    [Authorize]
    [ProducesResponseType<IntentionDonDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SoumettreIntention(
        Guid id, SoumettreIntentionDonCommand command, CancellationToken cancellationToken)
    {
        var cmd = command with { DemandeQuotaId = id };
        var result = await _mediator.Send(cmd, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    /// <summary>Accepte une intention de don (Coordinateur/Responsable).</summary>
    [HttpPatch("{id:guid}/intentions/{intentionId:guid}/accepter")]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType<DemandeQuotaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AccepterIntention(
        Guid id, Guid intentionId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AccepterIntentionDonCommand(id, intentionId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Refuse une intention de don (Coordinateur/Responsable).</summary>
    [HttpPatch("{id:guid}/intentions/{intentionId:guid}/refuser")]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType<DemandeQuotaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RefuserIntention(
        Guid id, Guid intentionId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RefuserIntentionDonCommand(id, intentionId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Confirme une intention de don (promesse tenue — don effectivement déposé).</summary>
    [HttpPatch("{id:guid}/intentions/{intentionId:guid}/confirmer")]
    [Authorize]
    [ProducesResponseType<DemandeQuotaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmerIntention(
        Guid id, Guid intentionId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ConfirmerIntentionDonCommand(id, intentionId), cancellationToken);
        return Ok(result);
    }
}
