using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.DemandeSurCatalogue.AjouterLigneCatalogue;
using CrisisConnect.Application.UseCases.DemandeSurCatalogue.CreateDemandeSurCatalogue;
using CrisisConnect.Application.UseCases.DemandeSurCatalogue.GetDemandeSurCatalogueById;
using CrisisConnect.Application.UseCases.DemandeSurCatalogue.GetDemandeSurCatalogues;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrisisConnect.API.Controllers;

/// <summary>Demandes avec intégration catalogue e-commerce (§5.1.3).</summary>
[ApiController]
[Route("api/demandes-sur-catalogue")]
public class DemandesSurCatalogueController : ControllerBase
{
    private readonly IMediator _mediator;

    public DemandesSurCatalogueController(IMediator mediator) => _mediator = mediator;

    /// <summary>Liste toutes les demandes sur catalogue.</summary>
    [HttpGet]
    public async Task<ActionResult<List<DemandeSurCatalogueDto>>> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetDemandeSurCataloguesQuery(), ct));

    /// <summary>Récupère une demande sur catalogue par ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DemandeSurCatalogueDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDemandeSurCatalogueByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Crée une demande sur catalogue.</summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<DemandeSurCatalogueDto>> Create(
        [FromBody] CreateDemandeSurCatalogueRequest request, CancellationToken ct)
    {
        var command = new CreateDemandeSurCatalogueCommand(
            request.Titre, request.Description, request.CreePar, request.UrlCatalogue);
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Ajoute une ligne au catalogue d'une demande.</summary>
    [HttpPost("{id:guid}/lignes")]
    [Authorize]
    public async Task<ActionResult<LigneCatalogueDto>> AjouterLigne(
        Guid id, [FromBody] AjouterLigneCatalogueRequest request, CancellationToken ct)
    {
        var command = new AjouterLigneCatalogueCommand(
            id, request.Reference, request.Designation, request.Quantite, request.PrixUnitaire, request.UrlProduit);
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}

public record CreateDemandeSurCatalogueRequest(
    string Titre,
    string Description,
    Guid CreePar,
    string UrlCatalogue);

public record AjouterLigneCatalogueRequest(
    string Reference,
    string Designation,
    int Quantite,
    double PrixUnitaire,
    string? UrlProduit);
