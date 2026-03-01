namespace CrisisConnect.Web.Models;

public record EntiteModel(
    Guid Id,
    string Email,
    string Nom,
    string Description,
    string MoyensContact,
    string? UrlPagePresentation,
    bool EstActive,
    Guid ResponsableId);
