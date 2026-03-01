using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Suggestions.GenererSuggestions;

/// <summary>
/// Génère automatiquement des suggestions d'appariement offre ↔ demande
/// pour une demande active donnée, basées sur la similarité textuelle,
/// la localisation et le niveau d'urgence.
/// </summary>
public record GenererSuggestionsCommand(Guid DemandeId)
    : IRequest<IReadOnlyList<SuggestionAppariementDto>>;
