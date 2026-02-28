using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Propositions.GetPropositionById;

public record GetPropositionByIdQuery(Guid Id) : IRequest<PropositionDto>;
