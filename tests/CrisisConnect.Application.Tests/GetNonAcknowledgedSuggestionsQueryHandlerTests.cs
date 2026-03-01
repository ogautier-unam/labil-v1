using AutoMapper;
using CrisisConnect.Application.UseCases.Suggestions.GetNonAcknowledgedSuggestions;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetNonAcknowledgedSuggestionsQueryHandlerTests
{
    private readonly ISuggestionAppariementRepository _suggestionRepo = Substitute.For<ISuggestionAppariementRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private GetNonAcknowledgedSuggestionsQueryHandler CréerHandler() => new(_suggestionRepo, _mapper);

    [Fact]
    public async Task GetNonAcknowledged_TroisSuggestions_RetourneTroisDtos()
    {
        // Arrange
        var suggestions = new List<SuggestionAppariement>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), 0.95, "Score élevé"),
            new(Guid.NewGuid(), Guid.NewGuid(), 0.80, "Score moyen"),
            new(Guid.NewGuid(), Guid.NewGuid(), 0.65, "Score faible")
        };
        _suggestionRepo.GetNonAcknowledgedAsync(Arg.Any<CancellationToken>())
            .Returns(suggestions.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetNonAcknowledgedSuggestionsQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.All(result, s => Assert.False(s.EstAcknowledged));
    }

    [Fact]
    public async Task GetNonAcknowledged_AucuneSuggestion_RetourneListeVide()
    {
        // Arrange
        _suggestionRepo.GetNonAcknowledgedAsync(Arg.Any<CancellationToken>())
            .Returns(Array.Empty<SuggestionAppariement>());

        // Act
        var result = await CréerHandler().Handle(new GetNonAcknowledgedSuggestionsQuery(), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
