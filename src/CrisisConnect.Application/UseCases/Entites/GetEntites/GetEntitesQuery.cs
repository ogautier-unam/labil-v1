using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Entites.GetEntites;

public record GetEntitesQuery : IRequest<IReadOnlyList<EntiteDto>>;
