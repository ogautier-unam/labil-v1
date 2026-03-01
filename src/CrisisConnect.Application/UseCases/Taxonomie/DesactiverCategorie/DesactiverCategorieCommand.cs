using MediatR;

namespace CrisisConnect.Application.UseCases.Taxonomie.DesactiverCategorie;

public record DesactiverCategorieCommand(Guid CategorieId) : IRequest;
