using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Propositions.CreateProposition;
using CrisisConnect.Application.UseCases.Propositions.GetPropositionById;
using CrisisConnect.Application.UseCases.Propositions.GetPropositions;
using MediatR;
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

    /// <summary>Liste toutes les propositions.</summary>
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

    /// <summary>Crée une nouvelle proposition.</summary>
    [HttpPost]
    [ProducesResponseType<PropositionDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePropositionCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
