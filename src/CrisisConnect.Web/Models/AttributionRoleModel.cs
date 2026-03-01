namespace CrisisConnect.Web.Models;

public record AttributionRoleModel(
    Guid Id,
    Guid ActeurId,
    string TypeRole,
    string Statut,
    DateTime DateDebut,
    DateTime? DateFin,
    bool Reconductible,
    Guid? AccrediteeParId);
