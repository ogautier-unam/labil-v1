namespace CrisisConnect.Web.Models;

public record TransactionModel(
    Guid Id,
    Guid PropositionId,
    Guid InitiateurId,
    string Statut,
    DateTime DateCreation);
