namespace CrisisConnect.Web.Models;

public record PropositionAvecValidationModel(
    Guid Id,
    string Titre,
    string Description,
    string Statut,
    Guid CreePar,
    DateTime CreeLe,
    string DescriptionValidation,
    string StatutValidation,
    Guid? ValideurEntiteId);
