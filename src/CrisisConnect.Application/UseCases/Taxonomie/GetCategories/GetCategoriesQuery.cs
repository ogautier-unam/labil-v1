using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Taxonomie.GetCategories;

public record GetCategoriesQuery(Guid ConfigId, string? Langue = "fr") : IRequest<IReadOnlyList<CategorieTaxonomieDto>>;
