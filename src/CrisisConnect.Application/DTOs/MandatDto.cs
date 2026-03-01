namespace CrisisConnect.Application.DTOs;

public record MandatDto(
    Guid Id,
    Guid MandantId,
    Guid MandataireId,
    string Portee,
    string Description,
    bool EstPublic,
    bool EstActif,
    DateTime DateDebut,
    DateTime? DateFin);
