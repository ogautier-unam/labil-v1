namespace CrisisConnect.Application.DTOs;

public record ConfigCatastropheDto(
    Guid Id,
    string Nom,
    string Description,
    string ZoneGeographique,
    string EtatReferent,
    bool EstActive,
    DateTime DateDebut,
    int DelaiArchivageJours,
    int DelaiRappelAvantArchivage,
    string LanguesActives,
    string ModesIdentificationActifs);
