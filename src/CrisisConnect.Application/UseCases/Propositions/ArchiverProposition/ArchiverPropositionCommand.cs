using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.ArchiverProposition;

public record ArchiverPropositionCommand(Guid PropositionId) : ICommand;
