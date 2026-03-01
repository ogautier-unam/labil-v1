using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Taxonomie.CreateCategorie;

public record CreateCategorieCommand(
    string Code,
    string NomJson,
    Guid ConfigId,
    Guid? ParentId = null,
    string DescriptionJson = "{}") : IRequest<CategorieTaxonomieDto>;
