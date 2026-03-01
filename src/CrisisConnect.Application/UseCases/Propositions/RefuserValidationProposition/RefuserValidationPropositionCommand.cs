using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.RefuserValidationProposition;

public record RefuserValidationPropositionCommand(Guid Id, Guid ValideurEntiteId) : IRequest<PropositionAvecValidationDto>;
