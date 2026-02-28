namespace CrisisConnect.Web.Models;

public record PanierModel(
    Guid Id,
    Guid ProprietaireId,
    string Statut,
    DateTime DateCreation,
    DateTime? DateConfirmation,
    IReadOnlyList<OffreModel> Offres);
