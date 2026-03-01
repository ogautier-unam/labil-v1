using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Suggestions.GetNonAcknowledgedSuggestions;

public record GetNonAcknowledgedSuggestionsQuery : IRequest<IReadOnlyList<SuggestionAppariementDto>>;
