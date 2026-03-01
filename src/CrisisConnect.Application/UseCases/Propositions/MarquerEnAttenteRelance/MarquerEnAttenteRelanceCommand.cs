using MediatR;

namespace CrisisConnect.Application.UseCases.Propositions.MarquerEnAttenteRelance;

public record MarquerEnAttenteRelanceCommand(Guid PropositionId) : IRequest;
