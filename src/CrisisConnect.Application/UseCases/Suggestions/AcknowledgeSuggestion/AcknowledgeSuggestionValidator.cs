using FluentValidation;

namespace CrisisConnect.Application.UseCases.Suggestions.AcknowledgeSuggestion;

public class AcknowledgeSuggestionValidator : AbstractValidator<AcknowledgeSuggestionCommand>
{
    public AcknowledgeSuggestionValidator()
    {
        RuleFor(x => x.SuggestionId).NotEmpty();
    }
}
