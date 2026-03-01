using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.CloreProposition;

public record ClorePropositionCommand(Guid PropositionId) : ICommand;
