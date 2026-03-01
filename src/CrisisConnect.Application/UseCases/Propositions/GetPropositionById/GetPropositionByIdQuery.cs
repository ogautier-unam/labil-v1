using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.GetPropositionById;

public record GetPropositionByIdQuery(Guid Id) : IRequest<PropositionDto>;
