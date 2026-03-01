namespace CrisisConnect.Web.Models;

public record PersonneModel(
    Guid Id,
    string Email,
    string Role,
    string Prenom,
    string Nom,
    string NomComplet,
    string? Telephone,
    string? UrlPhoto,
    string? LanguePreferee,
    string? MoyensContact,
    string? Rue,
    string? Ville,
    string? CodePostal,
    string? Pays,
    string NiveauBadge);
