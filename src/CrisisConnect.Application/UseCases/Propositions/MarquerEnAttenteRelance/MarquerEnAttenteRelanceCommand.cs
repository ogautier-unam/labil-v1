using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.MarquerEnAttenteRelance;

public record MarquerEnAttenteRelanceCommand(Guid PropositionId) : ICommand;
