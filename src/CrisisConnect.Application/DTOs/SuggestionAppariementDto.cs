namespace CrisisConnect.Application.DTOs;

public record SuggestionAppariementDto(
    Guid Id,
    Guid OffreId,
    Guid DemandeId,
    double ScoreCorrespondance,
    string Raisonnement,
    bool EstAcknowledged,
    DateTime DateGeneration);
