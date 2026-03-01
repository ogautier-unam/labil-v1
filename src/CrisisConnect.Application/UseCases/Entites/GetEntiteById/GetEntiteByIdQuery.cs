using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Entites.GetEntiteById;

public record GetEntiteByIdQuery(Guid Id) : IQuery<EntiteDto>;
