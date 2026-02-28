namespace CrisisConnect.Web.Models;

public record OffreModel(
    Guid Id,
    string Titre,
    string Description,
    string Statut,
    Guid CreePar,
    DateTime CreeLe,
    bool LivraisonIncluse);
