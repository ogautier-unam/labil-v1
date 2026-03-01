using CrisisConnect.Application.UseCases.Suggestions.AcknowledgeSuggestion;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class AcknowledgeSuggestionCommandHandlerTests
{
    private readonly ISuggestionAppariementRepository _suggestionRepo = Substitute.For<ISuggestionAppariementRepository>();

    private AcknowledgeSuggestionCommandHandler CréerHandler() => new(_suggestionRepo);

    [Fact]
    public async Task Acknowledge_SuggestionExiste_MarquéeEtPersistée()
    {
        // Arrange
        var suggestion = new SuggestionAppariement(Guid.NewGuid(), Guid.NewGuid(), 0.85, "Appariement géographique");
        _suggestionRepo.GetByIdAsync(suggestion.Id, Arg.Any<CancellationToken>())
            .Returns(suggestion);

        // Act
        await CréerHandler().Handle(new AcknowledgeSuggestionCommand(suggestion.Id), CancellationToken.None);

        // Assert
        Assert.True(suggestion.EstAcknowledged);
        await _suggestionRepo.Received(1).UpdateAsync(suggestion, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Acknowledge_SuggestionIntrouvable_LèveNotFoundException()
    {
        // Arrange
        var suggestionId = Guid.NewGuid();
        _suggestionRepo.GetByIdAsync(suggestionId, Arg.Any<CancellationToken>())
            .Returns((SuggestionAppariement?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new AcknowledgeSuggestionCommand(suggestionId), CancellationToken.None).AsTask());
    }

    [Fact]
    public async Task Acknowledge_SuggestionDéjàAcquittée_ResteAcquittéeEtPersistée()
    {
        // Arrange
        var suggestion = new SuggestionAppariement(Guid.NewGuid(), Guid.NewGuid(), 0.9, "Déjà vu");
        suggestion.Acknowledger(); // déjà acquittée
        _suggestionRepo.GetByIdAsync(suggestion.Id, Arg.Any<CancellationToken>())
            .Returns(suggestion);

        // Act
        await CréerHandler().Handle(new AcknowledgeSuggestionCommand(suggestion.Id), CancellationToken.None);

        // Assert — reste true, UpdateAsync toujours appelé
        Assert.True(suggestion.EstAcknowledged);
        await _suggestionRepo.Received(1).UpdateAsync(suggestion, Arg.Any<CancellationToken>());
    }
}
