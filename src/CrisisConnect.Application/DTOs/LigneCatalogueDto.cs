using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record LigneCatalogueDto(
    Guid Id,
    Guid DemandeSurCatalogueId,
    string Reference,
    string Designation,
    int Quantite,
    double PrixUnitaire,
    string? UrlProduit,
    StatutLigne Statut);
