namespace CrisisConnect.Web.Models;

public record CreerMandatRequest(
    Guid MandantId,
    Guid MandataireId,
    string Portee,
    string Description,
    bool EstPublic,
    DateTime DateDebut,
    DateTime? DateFin = null);
