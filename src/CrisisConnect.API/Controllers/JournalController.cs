using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Journal.GetEntreesJournal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/journal")]
[Authorize]
public class JournalController : ControllerBase
{
    private readonly IMediator _mediator;

    public JournalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Retourne le journal d'audit d'un acteur (ses propres entrées).</summary>
    [HttpGet("{acteurId:guid}")]
    [ProducesResponseType<IReadOnlyList<EntreeJournalDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByActeur(Guid acteurId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEntreesJournalQuery(acteurId), cancellationToken);
        return Ok(result);
    }
}
