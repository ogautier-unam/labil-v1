using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.ReconfirmerProposition;

public record ReconfirmerPropositionCommand(Guid PropositionId) : ICommand;
