using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Taxonomie.CreateCategorie;
using CrisisConnect.Application.UseCases.Taxonomie.DesactiverCategorie;
using CrisisConnect.Application.UseCases.Taxonomie.GetCategories;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

[ApiController]
[Route("api/taxonomie")]
public class TaxonomieController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaxonomieController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Retourne les catégories racines d'une configuration de crise, avec noms localisés (NF-04).</summary>
    /// <param name="configId">Identifiant de la configuration de crise.</param>
    /// <param name="langue">Code langue ISO 639-1 (ex. "fr", "en"). Défaut : "fr".</param>
    [HttpGet("config/{configId:guid}")]
    [ProducesResponseType<IReadOnlyList<CategorieTaxonomieDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRacines(Guid configId, [FromQuery] string? langue, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoriesQuery(configId, langue), cancellationToken);
        return Ok(result);
    }

    /// <summary>Crée une nouvelle catégorie (Coordinateur/Responsable uniquement).</summary>
    [HttpPost]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType<CategorieTaxonomieDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCategorieCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetRacines), new { configId = result.ConfigId }, result);
    }

    /// <summary>Désactive une catégorie (Coordinateur/Responsable uniquement).</summary>
    [HttpPatch("{id:guid}/desactiver")]
    [Authorize(Roles = "Coordinateur,Responsable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desactiver(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DesactiverCategorieCommand(id), cancellationToken);
        return NoContent();
    }
}
