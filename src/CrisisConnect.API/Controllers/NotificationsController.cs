using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Notifications.GetNotifications;
using CrisisConnect.Application.UseCases.Notifications.MarkAsRead;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Retourne les notifications d'un acteur.</summary>
    [HttpGet("{destinataireId:guid}")]
    [ProducesResponseType<IReadOnlyList<NotificationDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDestinataire(Guid destinataireId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetNotificationsQuery(destinataireId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Marque une notification comme lue.</summary>
    [HttpPatch("{id:guid}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new MarkNotificationAsReadCommand(id), cancellationToken);
        return NoContent();
    }
}
