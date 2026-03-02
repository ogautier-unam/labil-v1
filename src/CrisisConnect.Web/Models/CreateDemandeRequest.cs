namespace CrisisConnect.Web.Models;

public record CreateDemandeRequest(
    string Titre,
    string Description,
    Guid CreePar,
    string Urgence,
    string? RegionSeverite = null,
    bool EstRecurrente = false,
    string? FrequenceRecurrence = null);
