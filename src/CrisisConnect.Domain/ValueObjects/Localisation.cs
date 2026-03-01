namespace CrisisConnect.Domain.ValueObjects;

/// <summary>Coordonnées géographiques avec libellé adresse optionnel (L9).</summary>
public record Localisation(double Latitude, double Longitude, string? AdresseLibelle = null);
