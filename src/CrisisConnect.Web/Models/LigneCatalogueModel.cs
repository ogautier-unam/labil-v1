namespace CrisisConnect.Web.Models;

public record LigneCatalogueModel(
    Guid Id,
    Guid DemandeSurCatalogueId,
    string Reference,
    string Designation,
    int Quantite,
    double PrixUnitaire,
    string? UrlProduit,
    string Statut);
