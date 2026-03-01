using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Mandats.GetMandats;

public record GetMandatsQuery(Guid ActeurId) : IRequest<IReadOnlyList<MandatDto>>;
