using CrisisConnect.Domain.ValueObjects;

namespace CrisisConnect.Domain.Entities;

/// <summary>
/// Demande avec intégration d'un catalogue e-commerce tiers.
/// §5.1.3 : listing chiffré actualisé en temps réel.
/// Ex. école de Clairval (craies, tables, ordi) — §5.4.3.
/// </summary>
public class DemandeSurCatalogue : Demande
{
    public string UrlCatalogue { get; private set; } = string.Empty;

    private readonly List<LigneCatalogue> _lignes = [];
    public IReadOnlyCollection<LigneCatalogue> Lignes => _lignes.AsReadOnly();

    protected DemandeSurCatalogue() { }

    public DemandeSurCatalogue(string titre, string description, Guid creePar,
        string urlCatalogue, Localisation? localisation = null)
        : base(titre, description, creePar, localisation: localisation)
    {
        UrlCatalogue = urlCatalogue;
    }

    public LigneCatalogue AjouterLigne(string reference, string designation, int quantite, double prixUnitaire, string? urlProduit = null)
    {
        var ligne = new LigneCatalogue(Id, reference, designation, quantite, prixUnitaire, urlProduit);
        _lignes.Add(ligne);
        return ligne;
    }
}
