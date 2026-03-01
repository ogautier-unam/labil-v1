namespace CrisisConnect.Web.Models;

public record EntreeJournalModel(
    Guid Id,
    Guid ActeurId,
    string TypeOperation,
    DateTime DateHeure,
    string? Details);
