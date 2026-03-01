using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Enums;
using MediatR;

namespace CrisisConnect.Application.UseCases.Demandes.GetDemandes;

public record GetDemandesQuery(
    StatutProposition? Statut = null,
    NiveauUrgence? Urgence = null)
    : IRequest<IReadOnlyList<DemandeDto>>;
