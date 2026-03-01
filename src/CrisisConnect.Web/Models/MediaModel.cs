namespace CrisisConnect.Web.Models;

public record MediaModel(
    Guid Id,
    Guid PropositionId,
    string Url,
    string Type,
    DateTime DateAjout);
