namespace CrisisConnect.Application.DTOs;

public record CategorieTaxonomieDto(
    Guid Id,
    string Code,
    string NomJson,
    string DescriptionJson,
    bool EstActive,
    Guid? ParentId,
    Guid ConfigId,
    /// <summary>Nom localisé selon la langue demandée (extrait de NomJson).</summary>
    string Nom,
    /// <summary>Description localisée selon la langue demandée (extraite de DescriptionJson).</summary>
    string Description);
