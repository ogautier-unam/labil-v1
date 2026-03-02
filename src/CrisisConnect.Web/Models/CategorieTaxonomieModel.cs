namespace CrisisConnect.Web.Models;

public record CategorieTaxonomieModel(
    Guid Id,
    string Code,
    string NomJson,
    string DescriptionJson,
    bool EstActive,
    Guid? ParentId,
    Guid ConfigId,
    string Nom = "",
    string Description = "");
