using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.ValiderProposition;

public record ValiderPropositionCommand(Guid Id, Guid ValideurEntiteId) : IRequest<PropositionAvecValidationDto>;
