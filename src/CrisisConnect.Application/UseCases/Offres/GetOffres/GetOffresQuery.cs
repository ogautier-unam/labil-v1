using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Enums;
using MediatR;

namespace CrisisConnect.Application.UseCases.Offres.GetOffres;

public record GetOffresQuery(StatutProposition? Statut = null) : IRequest<IReadOnlyList<OffreDto>>;
