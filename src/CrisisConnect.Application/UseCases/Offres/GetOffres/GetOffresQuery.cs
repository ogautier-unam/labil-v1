using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Offres.GetOffres;

public record GetOffresQuery() : IRequest<IReadOnlyList<OffreDto>>;
