using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Transactions.GetDiscussion;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetDiscussionQueryHandlerTests
{
    private readonly ITransactionRepository _transactionRepo = Substitute.For<ITransactionRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetDiscussionQueryHandler CréerHandler() => new(_transactionRepo, _mapper);

    [Fact]
    public async Task GetDiscussion_TransactionAvecMessages_RetourneDiscussionDto()
    {
        // Arrange
        var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid());
        transaction.Discussion.AjouterMessage(Guid.NewGuid(), "Bonjour", "fr");
        _transactionRepo.GetByIdAsync(transaction.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);

        // Act
        var result = await CréerHandler().Handle(new GetDiscussionQuery(transaction.Id), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(transaction.Discussion.Id, result.Id);
        Assert.Equal(transaction.Id, result.TransactionId);
        Assert.Single(result.Messages);
        Assert.Equal("Bonjour", result.Messages[0].Contenu);
    }

    [Fact]
    public async Task GetDiscussion_TransactionIntrouvable_LèveNotFoundException()
    {
        // Arrange
        _transactionRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Transaction?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => CréerHandler().Handle(new GetDiscussionQuery(Guid.NewGuid()), CancellationToken.None).AsTask());
    }
}
