using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.DemandeRepartitionGeo.CreateDemandeRepartitionGeo;
using CrisisConnect.Application.UseCases.DemandeRepartitionGeo.GetDemandeRepartitionGeoById;
using CrisisConnect.Application.UseCases.DemandeRepartitionGeo.GetDemandesRepartitionGeo;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

/// <summary>Demandes géo-répartition (§5.1.3 — outil GIS).</summary>
[ApiController]
[Route("api/demandes-repartition-geo")]
public class DemandesRepartitionGeoController : ControllerBase
{
    private readonly IMediator _mediator;

    public DemandesRepartitionGeoController(IMediator mediator) => _mediator = mediator;

    /// <summary>Liste toutes les demandes de répartition géographique.</summary>
    [HttpGet]
    public async Task<ActionResult<List<DemandeRepartitionGeoDto>>> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetDemandesRepartitionGeoQuery(), ct));

    /// <summary>Récupère une demande de répartition géographique par ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DemandeRepartitionGeoDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDemandeRepartitionGeoByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Crée une demande de répartition géographique.</summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<DemandeRepartitionGeoDto>> Create(
        [FromBody] CreateDemandeRepartitionGeoRequest request, CancellationToken ct)
    {
        var command = new CreateDemandeRepartitionGeoCommand(
            request.Titre, request.Description, request.CreePar,
            request.NombreRessourcesRequises, request.DescriptionMission);
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}

public record CreateDemandeRepartitionGeoRequest(
    string Titre,
    string Description,
    Guid CreePar,
    int NombreRessourcesRequises,
    string DescriptionMission);
