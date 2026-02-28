using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Demandes.GetDemandes;

public record GetDemandesQuery() : IRequest<IReadOnlyList<DemandeDto>>;
