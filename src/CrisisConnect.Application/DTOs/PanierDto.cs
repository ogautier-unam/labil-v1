namespace CrisisConnect.Application.DTOs;

public record PanierDto(
    Guid Id,
    Guid ProprietaireId,
    string Statut,
    DateTime DateCreation,
    DateTime? DateConfirmation,
    IReadOnlyList<OffreDto> Offres);
