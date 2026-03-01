using CrisisConnect.Application.UseCases.Suggestions.GenererSuggestions;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class GenererSuggestionsValidatorTests
{
    private readonly GenererSuggestionsValidator _validator = new();

    [Fact]
    public void Valide_DemandeIdRempli_PasseValidation()
    {
        _validator.TestValidate(new GenererSuggestionsCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_DemandeIdVide_EchecSurDemandeId()
    {
        _validator.TestValidate(new GenererSuggestionsCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.DemandeId);
    }
}
