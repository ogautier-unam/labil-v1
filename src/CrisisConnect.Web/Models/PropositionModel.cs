namespace CrisisConnect.Web.Models;

public record PropositionModel(
    Guid Id,
    string Titre,
    string Description,
    string Statut,
    Guid CreePar,
    DateTime CreeLe);
