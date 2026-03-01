using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record DemandeSurCatalogueDto(
    Guid Id,
    string Titre,
    string Description,
    StatutProposition Statut,
    Guid CreePar,
    DateTime CreeLe,
    string UrlCatalogue,
    List<LigneCatalogueDto> Lignes);
