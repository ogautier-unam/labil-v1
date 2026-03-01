namespace CrisisConnect.Application.DTOs;

public record CategorieTaxonomieDto(
    Guid Id,
    string Code,
    string NomJson,
    string DescriptionJson,
    bool EstActive,
    Guid? ParentId,
    Guid ConfigId);
