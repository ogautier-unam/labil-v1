using MediatR;

namespace CrisisConnect.Application.UseCases.Suggestions.AcknowledgeSuggestion;

public record AcknowledgeSuggestionCommand(Guid SuggestionId) : IRequest;
