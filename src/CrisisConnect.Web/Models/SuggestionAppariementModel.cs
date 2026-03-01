namespace CrisisConnect.Web.Models;

public record SuggestionAppariementModel(
    Guid Id,
    Guid OffreId,
    Guid DemandeId,
    double ScoreCorrespondance,
    string Raisonnement,
    bool EstAcknowledged,
    DateTime DateGeneration);
