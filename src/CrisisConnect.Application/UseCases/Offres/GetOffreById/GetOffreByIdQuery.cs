using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Offres.GetOffreById;

public record GetOffreByIdQuery(Guid Id) : IRequest<OffreDto>;
