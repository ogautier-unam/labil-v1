using MediatR;

namespace CrisisConnect.Application.UseCases.Propositions.ArchiverProposition;

public record ArchiverPropositionCommand(Guid PropositionId) : IRequest;
