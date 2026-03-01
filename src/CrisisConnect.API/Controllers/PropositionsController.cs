using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Demandes.CreateDemande;
using CrisisConnect.Application.UseCases.Demandes.GetDemandeById;
using CrisisConnect.Application.UseCases.Demandes.GetDemandes;
using CrisisConnect.Application.UseCases.Offres.CreateOffre;
using CrisisConnect.Application.UseCases.Offres.GetOffreById;
using CrisisConnect.Application.UseCases.Offres.GetOffres;
using CrisisConnect.Application.UseCases.Propositions.ArchiverProposition;
using CrisisConnect.Application.UseCases.Propositions.CloreProposition;
using CrisisConnect.Application.UseCases.Propositions.GetPropositionById;
using CrisisConnect.Application.UseCases.Propositions.GetPropositions;
using CrisisConnect.Application.UseCases.Propositions.MarquerEnAttenteRelance;
using CrisisConnect.Application.UseCases.Propositions.ReconfirmerProposition;
using CrisisConnect.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropositionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PropositionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Liste toutes les propositions (offres et demandes).</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<PropositionDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPropositionsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Retourne une proposition par son identifiant.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<PropositionDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPropositionByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    // ── Offres ──────────────────────────────────────────────────────────────

    /// <summary>Liste toutes les offres, avec filtre optionnel par statut.</summary>
    [HttpGet("offres")]
    [ProducesResponseType<IReadOnlyList<OffreDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOffres(
        [FromQuery] StatutProposition? statut,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOffresQuery(statut), cancellationToken);
        return Ok(result);
    }

    /// <summary>Retourne une offre par son identifiant.</summary>
    [HttpGet("offres/{id:guid}")]
    [ProducesResponseType<OffreDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOffreById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOffreByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Crée une nouvelle offre.</summary>
    [HttpPost("offres")]
    [Authorize]
    [ProducesResponseType<OffreDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateOffre([FromBody] CreateOffreCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetOffreById), new { id = result.Id }, result);
    }

    // ── Demandes ─────────────────────────────────────────────────────────────

    /// <summary>Liste toutes les demandes, avec filtres optionnels par statut et urgence.</summary>
    [HttpGet("demandes")]
    [ProducesResponseType<IReadOnlyList<DemandeDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDemandes(
        [FromQuery] StatutProposition? statut,
        [FromQuery] NiveauUrgence? urgence,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDemandesQuery(statut, urgence), cancellationToken);
        return Ok(result);
    }

    /// <summary>Retourne une demande par son identifiant.</summary>
    [HttpGet("demandes/{id:guid}")]
    [ProducesResponseType<DemandeDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDemandeById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDemandeByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Crée une nouvelle demande.</summary>
    [HttpPost("demandes")]
    [Authorize]
    [ProducesResponseType<DemandeDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateDemande([FromBody] CreateDemandeCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetDemandeById), new { id = result.Id }, result);
    }

    // ── Cycle de vie ──────────────────────────────────────────────────────────

    /// <summary>Clôture une proposition (statut → Cloturée). Coordinateur ou Responsable.</summary>
    [HttpPatch("{id:guid}/clore")]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Clore(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ClorePropositionCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>Archive une proposition (statut → Archivée). Coordinateur ou Responsable.</summary>
    [HttpPatch("{id:guid}/archiver")]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Archiver(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ArchiverPropositionCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>Passe une proposition en attente de relance (statut → EnAttenteRelance).</summary>
    [HttpPatch("{id:guid}/relance")]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarquerRelance(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new MarquerEnAttenteRelanceCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>Reconfirme une proposition en attente de relance (statut → Active).</summary>
    [HttpPatch("{id:guid}/reconfirmer")]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Reconfirmer(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ReconfirmerPropositionCommand(id), cancellationToken);
        return NoContent();
    }
}
