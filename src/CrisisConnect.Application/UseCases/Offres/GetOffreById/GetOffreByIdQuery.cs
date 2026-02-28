using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Offres.GetOffreById;

public record GetOffreByIdQuery(Guid Id) : IRequest<OffreDto>;
