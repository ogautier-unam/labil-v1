using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.GetPropositions;

public record GetPropositionsQuery : IRequest<IReadOnlyList<PropositionDto>>;
