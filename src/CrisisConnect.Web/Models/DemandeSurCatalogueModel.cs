namespace CrisisConnect.Web.Models;

public record DemandeSurCatalogueModel(
    Guid Id,
    string Titre,
    string Description,
    string Statut,
    Guid CreePar,
    DateTime CreeLe,
    string UrlCatalogue,
    List<LigneCatalogueModel> Lignes);
