using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Suggestions.GetNonAcknowledgedSuggestions;

public record GetNonAcknowledgedSuggestionsQuery : IRequest<IReadOnlyList<SuggestionAppariementDto>>;
