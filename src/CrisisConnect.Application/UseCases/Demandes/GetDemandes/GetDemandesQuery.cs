using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Enums;
using Mediator;

namespace CrisisConnect.Application.UseCases.Demandes.GetDemandes;

public record GetDemandesQuery(
    StatutProposition? Statut = null,
    NiveauUrgence? Urgence = null,
    string? Strategie = null)
    : IRequest<IReadOnlyList<DemandeDto>>;
