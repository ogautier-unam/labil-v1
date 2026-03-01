using AutoMapper;
using CrisisConnect.Application.UseCases.Suggestions.GetSuggestionsByDemande;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetSuggestionsByDemandeQueryHandlerTests
{
    private readonly ISuggestionAppariementRepository _suggestionRepo = Substitute.For<ISuggestionAppariementRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private GetSuggestionsByDemandeQueryHandler CréerHandler() => new(_suggestionRepo, _mapper);

    [Fact]
    public async Task GetSuggestionsByDemande_DeuxSuggestions_RetourneDeuxDtos()
    {
        // Arrange
        var demandeId = Guid.NewGuid();
        var suggestions = new List<SuggestionAppariement>
        {
            new(Guid.NewGuid(), demandeId, 0.92, "Géolocalisation proche"),
            new(Guid.NewGuid(), demandeId, 0.75, "Nature identique")
        };
        _suggestionRepo.GetByDemandeAsync(demandeId, Arg.Any<CancellationToken>())
            .Returns(suggestions.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetSuggestionsByDemandeQuery(demandeId), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, s => s.ScoreCorrespondance == 0.92);
        Assert.Contains(result, s => s.Raisonnement == "Nature identique");
    }

    [Fact]
    public async Task GetSuggestionsByDemande_AucuneSuggestion_RetourneListeVide()
    {
        // Arrange
        var demandeId = Guid.NewGuid();
        _suggestionRepo.GetByDemandeAsync(demandeId, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<SuggestionAppariement>());

        // Act
        var result = await CréerHandler().Handle(new GetSuggestionsByDemandeQuery(demandeId), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
