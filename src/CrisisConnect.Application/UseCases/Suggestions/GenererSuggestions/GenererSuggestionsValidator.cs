using FluentValidation;

namespace CrisisConnect.Application.UseCases.Suggestions.GenererSuggestions;

public class GenererSuggestionsValidator : AbstractValidator<GenererSuggestionsCommand>
{
    public GenererSuggestionsValidator()
    {
        RuleFor(x => x.DemandeId)
            .NotEmpty().WithMessage("L'identifiant de la demande est requis.");
    }
}
