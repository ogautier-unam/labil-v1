using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Transactions.AnnulerTransaction;
using CrisisConnect.Application.UseCases.Transactions.BasculerVisibiliteDiscussion;
using CrisisConnect.Application.UseCases.Transactions.ConfirmerTransaction;
using CrisisConnect.Application.UseCases.Transactions.EnvoyerMessage;
using CrisisConnect.Application.UseCases.Transactions.GetDiscussion;
using CrisisConnect.Application.UseCases.Transactions.GetTransactionById;
using CrisisConnect.Application.UseCases.Transactions.GetTransactions;
using CrisisConnect.Application.UseCases.Transactions.InitierTransaction;
using CrisisConnect.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Liste toutes les transactions.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<TransactionDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTransactionsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Retourne une transaction par son identifiant.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<TransactionDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTransactionByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Initie une transaction sur une proposition.</summary>
    [HttpPost]
    [ProducesResponseType<TransactionDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Initier([FromBody] InitierTransactionCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Confirme une transaction (la proposition est clôturée).</summary>
    [HttpPatch("{id:guid}/confirmer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Confirmer(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ConfirmerTransactionCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>Annule une transaction (la proposition redevient disponible).</summary>
    [HttpPatch("{id:guid}/annuler")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Annuler(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AnnulerTransactionCommand(id), cancellationToken);
        return NoContent();
    }

    // ── Discussion ────────────────────────────────────────────────────────────

    /// <summary>Retourne la discussion (avec ses messages) d'une transaction.</summary>
    [HttpGet("{id:guid}/discussion")]
    [ProducesResponseType<DiscussionDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDiscussion(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDiscussionQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Envoie un message dans la discussion d'une transaction.</summary>
    [HttpPost("{id:guid}/messages")]
    [ProducesResponseType<MessageDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EnvoyerMessage(
        Guid id, [FromBody] EnvoyerMessageCommand command, CancellationToken cancellationToken)
    {
        // L'id de route prime sur le body pour éviter les injections d'ID
        var cmd = command with { TransactionId = id };
        var result = await _mediator.Send(cmd, cancellationToken);
        return CreatedAtAction(nameof(GetDiscussion), new { id }, result);
    }

    /// <summary>Bascule la visibilité de la discussion (Publique/Privee).</summary>
    [HttpPatch("{id:guid}/discussion/visibilite")]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BasculerVisibilite(
        Guid id, [FromQuery] Visibilite visibilite, CancellationToken cancellationToken)
    {
        await _mediator.Send(new BasculerVisibiliteDiscussionCommand(id, visibilite), cancellationToken);
        return NoContent();
    }
}
