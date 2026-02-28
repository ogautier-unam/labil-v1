namespace CrisisConnect.Domain.Entities;

/// <summary>
/// Taxonomie des propositions — Pattern Composite (GoF) : arbre catégories/sous-catégories.
/// §5 ex.3 : extensible par l'AC SANS redéploiement.
/// §5.3 : 9 catégories de base (Logement, Évacuation, Soins, Info, Emploi,
///         Aide financière, Assistance médicale, Fournitures, Aide logistique).
/// </summary>
public class CategorieTaxonomie
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Code { get; private set; } = string.Empty;
    /// <summary>Nom sérialisé en JSON : {"fr":"Logement","en":"Housing",...}</summary>
    public string NomJson { get; private set; } = "{}";
    /// <summary>Description sérialisée en JSON multilingue.</summary>
    public string DescriptionJson { get; private set; } = "{}";
    public bool EstActive { get; private set; } = true;
    public Guid? ParentId { get; private set; }
    public Guid ConfigId { get; private set; }

    private readonly List<CategorieTaxonomie> _sousCategories = [];
    public IReadOnlyCollection<CategorieTaxonomie> SousCategories => _sousCategories.AsReadOnly();

    protected CategorieTaxonomie() { }

    public CategorieTaxonomie(string code, string nomJson, Guid configId, Guid? parentId = null, string descriptionJson = "{}")
    {
        Code = code;
        NomJson = nomJson;
        DescriptionJson = descriptionJson;
        ConfigId = configId;
        ParentId = parentId;
    }

    public void Desactiver() => EstActive = false;
    public void Reactiver() => EstActive = true;
}
