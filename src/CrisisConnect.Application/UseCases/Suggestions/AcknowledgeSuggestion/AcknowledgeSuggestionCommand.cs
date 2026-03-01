using Mediator;

namespace CrisisConnect.Application.UseCases.Suggestions.AcknowledgeSuggestion;

public record AcknowledgeSuggestionCommand(Guid SuggestionId) : ICommand;
