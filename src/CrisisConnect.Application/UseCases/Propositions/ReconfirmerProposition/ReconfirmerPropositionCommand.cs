using MediatR;

namespace CrisisConnect.Application.UseCases.Propositions.ReconfirmerProposition;

public record ReconfirmerPropositionCommand(Guid PropositionId) : IRequest;
