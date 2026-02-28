using CrisisConnect.Domain.ValueObjects;

namespace CrisisConnect.Domain.Entities;

/// <summary>
/// Demande géo-répartition : localise des besoins géographiquement et distribue les ressources.
/// §5.1.3 : outil GIS pour répartir les ressources dans le temps et l'espace.
/// Ex. bénévoles nettoyage/évacuation boues (§5.4.5 Rochebranle).
/// </summary>
public class DemandeRepartitionGeo : Demande
{
    public int NombreRessourcesRequises { get; private set; }
    public string DescriptionMission { get; private set; } = string.Empty;

    protected DemandeRepartitionGeo() { }

    public DemandeRepartitionGeo(string titre, string description, Guid creePar,
        int nombreRessourcesRequises, string descriptionMission, Localisation? localisation = null)
        : base(titre, description, creePar, localisation: localisation)
    {
        NombreRessourcesRequises = nombreRessourcesRequises;
        DescriptionMission = descriptionMission;
    }
}
