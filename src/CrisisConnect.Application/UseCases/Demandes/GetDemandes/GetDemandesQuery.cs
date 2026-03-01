using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Enums;
using Mediator;

namespace CrisisConnect.Application.UseCases.Demandes.GetDemandes;

public record GetDemandesQuery(
    StatutProposition? Statut = null,
    NiveauUrgence? Urgence = null)
    : IRequest<IReadOnlyList<DemandeDto>>;
