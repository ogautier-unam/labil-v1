using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.RecyclerProposition;

public record RecyclerPropositionCommand(Guid PropositionId) : ICommand;
