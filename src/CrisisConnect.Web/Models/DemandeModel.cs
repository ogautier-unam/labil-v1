namespace CrisisConnect.Web.Models;

public record DemandeModel(
    Guid Id,
    string Titre,
    string Description,
    string Statut,
    Guid CreePar,
    DateTime CreeLe,
    string OperateurLogique,
    string Urgence,
    string? RegionSeverite);
