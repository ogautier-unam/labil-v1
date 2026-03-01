namespace CrisisConnect.Web.Models;

public record DemandeRepartitionGeoModel(
    Guid Id,
    string Titre,
    string Description,
    string Statut,
    Guid CreePar,
    DateTime CreeLe,
    int NombreRessourcesRequises,
    string DescriptionMission);
