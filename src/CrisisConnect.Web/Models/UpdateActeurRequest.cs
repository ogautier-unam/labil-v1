namespace CrisisConnect.Web.Models;

public record UpdateActeurRequest(
    string Prenom,
    string Nom,
    string? Telephone = null,
    string? UrlPhoto = null,
    string? LanguePreferee = null,
    string? MoyensContact = null,
    string? Rue = null,
    string? Ville = null,
    string? CodePostal = null,
    string? Pays = null);
