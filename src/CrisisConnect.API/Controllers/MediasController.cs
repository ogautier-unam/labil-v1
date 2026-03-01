using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Medias.AttacherMedia;
using CrisisConnect.Application.UseCases.Medias.GetMediasByProposition;
using CrisisConnect.Domain.Enums;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

/// <summary>Médias (photos, vidéos, audio, documents) attachés aux propositions (§5.1 ex.7).</summary>
[ApiController]
[Route("api/propositions/{propositionId:guid}/medias")]
public class MediasController : ControllerBase
{
    private readonly IMediator _mediator;

    public MediasController(IMediator mediator) => _mediator = mediator;

    /// <summary>Liste les médias d'une proposition.</summary>
    [HttpGet]
    public async Task<ActionResult<List<MediaDto>>> GetAll(Guid propositionId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMediasByPropositionQuery(propositionId), ct));

    /// <summary>Attache un média à une proposition via son URL.</summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<MediaDto>> Attacher(
        Guid propositionId, [FromBody] AttacherMediaRequest request, CancellationToken ct)
    {
        var command = new AttacherMediaCommand(propositionId, request.Url, request.Type);
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { propositionId }, result);
    }
}

public record AttacherMediaRequest(string Url, TypeMedia Type);
