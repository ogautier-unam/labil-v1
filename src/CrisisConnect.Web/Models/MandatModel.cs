namespace CrisisConnect.Web.Models;

public record MandatModel(
    Guid Id,
    Guid MandantId,
    Guid MandataireId,
    string Portee,
    string Description,
    bool EstPublic,
    bool EstActif,
    DateTime DateDebut,
    DateTime? DateFin);
