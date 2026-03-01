using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Entites.GetEntites;

public record GetEntitesQuery : IRequest<IReadOnlyList<EntiteDto>>;
