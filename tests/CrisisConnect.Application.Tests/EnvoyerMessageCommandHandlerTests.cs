using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Transactions.EnvoyerMessage;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class EnvoyerMessageCommandHandlerTests
{
    private readonly ITransactionRepository _transactionRepo = Substitute.For<ITransactionRepository>();
    private readonly IServiceTraduction _serviceTraduction = Substitute.For<IServiceTraduction>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private EnvoyerMessageCommandHandler CréerHandler() => new(_transactionRepo, _serviceTraduction, _mapper);

    [Fact]
    public async Task EnvoyerMessage_TransactionExistante_MessageAjoutéEtRetourné()
    {
        // Arrange
        var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid());
        var expediteurId = Guid.NewGuid();
        _transactionRepo.GetByIdAsync(transaction.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);

        var cmd = new EnvoyerMessageCommand(transaction.Id, expediteurId, "Bonjour", "fr");

        // Act
        var result = await CréerHandler().Handle(cmd, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Bonjour", result.Contenu);
        Assert.Equal(expediteurId, result.ExpediteurId);
        Assert.Equal("fr", result.Langue);
        Assert.Single(transaction.Discussion.Messages);
        await _transactionRepo.Received(1).UpdateAsync(transaction, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task EnvoyerMessage_TransactionIntrouvable_LèveNotFoundException()
    {
        // Arrange
        _transactionRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Transaction?)null);

        var cmd = new EnvoyerMessageCommand(Guid.NewGuid(), Guid.NewGuid(), "Bonjour");

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => CréerHandler().Handle(cmd, CancellationToken.None).AsTask());
    }
}
