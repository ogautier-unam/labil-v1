using CrisisConnect.Application.UseCases.Transactions.ConfirmerTransaction;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class ConfirmerTransactionCommandHandlerTests
{
    private readonly ITransactionRepository _transactionRepo = Substitute.For<ITransactionRepository>();
    private readonly IPropositionRepository _propositionRepo = Substitute.For<IPropositionRepository>();

    private ConfirmerTransactionCommandHandler CréerHandler() =>
        new(_transactionRepo, _propositionRepo);

    [Fact]
    public async Task ConfirmerTransaction_EnCours_ConfirméeEtPropositionClôturée()
    {
        // Arrange
        var offre = new Offre("Titre", "Desc", Guid.NewGuid());
        offre.MarquerEnTransaction();

        var transaction = new Transaction(offre.Id, Guid.NewGuid());
        _transactionRepo.GetByIdAsync(transaction.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);
        _propositionRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>())
            .Returns(offre);

        var handler = CréerHandler();

        // Act
        await handler.Handle(new ConfirmerTransactionCommand(transaction.Id), CancellationToken.None);

        // Assert
        Assert.Equal(Domain.Enums.StatutTransaction.Confirmee, transaction.Statut);
        Assert.Equal(Domain.Enums.StatutProposition.Cloturee, offre.Statut);
        await _transactionRepo.Received(1).UpdateAsync(transaction, Arg.Any<CancellationToken>());
        await _propositionRepo.Received(1).UpdateAsync(offre, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ConfirmerTransaction_Introuvable_LèveNotFoundException()
    {
        // Arrange
        _transactionRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Transaction?)null);

        var handler = CréerHandler();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new ConfirmerTransactionCommand(Guid.NewGuid()), CancellationToken.None).AsTask());
    }

    [Fact]
    public async Task ConfirmerTransaction_PropositionIntrouvable_SeulementTransactionMiseAJour()
    {
        // Arrange — proposition null : le handler doit confirmer la transaction sans lever d'exception
        var transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid());
        _transactionRepo.GetByIdAsync(transaction.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);
        _propositionRepo.GetByIdAsync(transaction.PropositionId, Arg.Any<CancellationToken>())
            .Returns((Proposition?)null);

        var handler = CréerHandler();

        // Act
        await handler.Handle(new ConfirmerTransactionCommand(transaction.Id), CancellationToken.None);

        // Assert
        Assert.Equal(Domain.Enums.StatutTransaction.Confirmee, transaction.Statut);
        await _transactionRepo.Received(1).UpdateAsync(transaction, Arg.Any<CancellationToken>());
        await _propositionRepo.DidNotReceive().UpdateAsync(Arg.Any<Proposition>(), Arg.Any<CancellationToken>());
    }
}
