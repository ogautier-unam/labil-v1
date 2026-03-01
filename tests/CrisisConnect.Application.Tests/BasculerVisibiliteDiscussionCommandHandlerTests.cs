using CrisisConnect.Application.UseCases.Transactions.BasculerVisibiliteDiscussion;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class BasculerVisibiliteDiscussionCommandHandlerTests
{
    private readonly ITransactionRepository _transactionRepo = Substitute.For<ITransactionRepository>();

    private BasculerVisibiliteDiscussionCommandHandler CréerHandler() => new(_transactionRepo);

    [Fact]
    public async Task BasculerVisibilite_TransactionExistante_VisibilitéMiseÀJour()
    {
        // Arrange
        var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid());
        _transactionRepo.GetByIdAsync(transaction.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);

        var cmd = new BasculerVisibiliteDiscussionCommand(transaction.Id, Visibilite.Privee);

        // Act
        await CréerHandler().Handle(cmd, CancellationToken.None);

        // Assert
        Assert.Equal(Visibilite.Privee, transaction.Discussion.Visibilite);
        await _transactionRepo.Received(1).UpdateAsync(transaction, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task BasculerVisibilite_TransactionIntrouvable_LèveNotFoundException()
    {
        // Arrange
        _transactionRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Transaction?)null);

        var cmd = new BasculerVisibiliteDiscussionCommand(Guid.NewGuid(), Visibilite.Privee);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => CréerHandler().Handle(cmd, CancellationToken.None).AsTask());
    }
}
