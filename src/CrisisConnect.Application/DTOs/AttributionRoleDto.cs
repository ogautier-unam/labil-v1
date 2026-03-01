namespace CrisisConnect.Application.DTOs;

public record AttributionRoleDto(
    Guid Id,
    Guid ActeurId,
    string TypeRole,
    string Statut,
    DateTime DateDebut,
    DateTime? DateFin,
    bool Reconductible,
    Guid? AccrediteeParId);
