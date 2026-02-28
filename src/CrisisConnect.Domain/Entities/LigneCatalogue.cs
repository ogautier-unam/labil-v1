using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Entities;

/// <summary>Ligne d'un catalogue e-commerce dans une DemandeSurCatalogue.</summary>
public class LigneCatalogue
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid DemandeSurCatalogueId { get; private set; }
    public string Reference { get; private set; } = string.Empty;
    public string Designation { get; private set; } = string.Empty;
    public int Quantite { get; private set; }
    public double PrixUnitaire { get; private set; }
    public string? UrlProduit { get; private set; }
    public StatutLigne Statut { get; private set; } = StatutLigne.Demandee;

    protected LigneCatalogue() { }

    public LigneCatalogue(Guid demandeSurCatalogueId, string reference, string designation,
        int quantite, double prixUnitaire, string? urlProduit = null)
    {
        DemandeSurCatalogueId = demandeSurCatalogueId;
        Reference = reference;
        Designation = designation;
        Quantite = quantite;
        PrixUnitaire = prixUnitaire;
        UrlProduit = urlProduit;
    }

    public void MarquerPartiellementFournie() => Statut = StatutLigne.PartiellementFournie;
    public void MarquerFournie() => Statut = StatutLigne.Fournie;
}
