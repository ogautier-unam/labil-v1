using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.ConfigCatastrophe.CreateConfigCatastrophe;
using CrisisConnect.Application.UseCases.ConfigCatastrophe.GetConfigCatastrophe;
using CrisisConnect.Application.UseCases.ConfigCatastrophe.UpdateConfigCatastrophe;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/config-catastrophe")]
public class ConfigCatastropheController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConfigCatastropheController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Retourne la configuration de crise active.</summary>
    [HttpGet]
    [ProducesResponseType<ConfigCatastropheDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetConfigCatastropheQuery(), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    /// <summary>Crée une nouvelle configuration de crise (Responsable uniquement).</summary>
    [HttpPost]
    [Authorize(Roles = "Responsable")]
    [ProducesResponseType<ConfigCatastropheDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateConfigCatastropheCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetActive), result);
    }

    /// <summary>Met à jour les paramètres d'une configuration de crise (Responsable uniquement).</summary>
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Responsable")]
    [ProducesResponseType<ConfigCatastropheDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateConfigCatastropheCommand command, CancellationToken cancellationToken)
    {
        var cmd = command with { Id = id };
        var result = await _mediator.Send(cmd, cancellationToken);
        return Ok(result);
    }
}
