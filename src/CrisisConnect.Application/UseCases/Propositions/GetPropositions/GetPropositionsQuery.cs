using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Propositions.GetPropositions;

public record GetPropositionsQuery : IRequest<IReadOnlyList<PropositionDto>>;
