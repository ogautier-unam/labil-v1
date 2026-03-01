namespace CrisisConnect.Application.DTOs;

public record EntiteDto(
    Guid Id,
    string Email,
    string Nom,
    string Description,
    string MoyensContact,
    string? UrlPagePresentation,
    bool EstActive,
    Guid ResponsableId);
