using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Propositions.CreateProposition;

public record CreatePropositionCommand(
    string Titre,
    string Description,
    Guid CreePar,
    double? Latitude = null,
    double? Longitude = null) : IRequest<PropositionDto>;
