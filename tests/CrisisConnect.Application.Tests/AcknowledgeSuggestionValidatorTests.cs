using CrisisConnect.Application.UseCases.Suggestions.AcknowledgeSuggestion;

namespace CrisisConnect.Application.Tests;

public class AcknowledgeSuggestionValidatorTests
{
    private readonly AcknowledgeSuggestionValidator _validator = new();

    [Fact]
    public async Task Valide_SuggestionIdRempli_PasseValidation()
    {
        var cmd = new AcknowledgeSuggestionCommand(Guid.NewGuid());
        var result = await _validator.ValidateAsync(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Invalide_SuggestionIdVide_EchecSurSuggestionId()
    {
        var cmd = new AcknowledgeSuggestionCommand(Guid.Empty);
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "SuggestionId");
    }
}
