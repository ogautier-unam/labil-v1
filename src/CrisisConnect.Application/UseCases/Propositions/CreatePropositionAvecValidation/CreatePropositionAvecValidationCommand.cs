using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Propositions.CreatePropositionAvecValidation;

public record CreatePropositionAvecValidationCommand(
    string Titre,
    string Description,
    Guid CreePar,
    string DescriptionValidation,
    double? Latitude = null,
    double? Longitude = null) : IRequest<PropositionAvecValidationDto>;
