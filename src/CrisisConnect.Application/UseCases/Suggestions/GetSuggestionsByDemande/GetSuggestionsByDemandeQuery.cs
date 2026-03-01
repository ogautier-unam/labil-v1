using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Suggestions.GetSuggestionsByDemande;

public record GetSuggestionsByDemandeQuery(Guid DemandeId) : IRequest<IReadOnlyList<SuggestionAppariementDto>>;
