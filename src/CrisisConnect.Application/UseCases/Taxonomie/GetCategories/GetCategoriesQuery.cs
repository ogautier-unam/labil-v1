using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Taxonomie.GetCategories;

public record GetCategoriesQuery(Guid ConfigId) : IRequest<IReadOnlyList<CategorieTaxonomieDto>>;
