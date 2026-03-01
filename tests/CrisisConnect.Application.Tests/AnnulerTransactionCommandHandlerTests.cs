using CrisisConnect.Application.UseCases.Transactions.AnnulerTransaction;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class AnnulerTransactionCommandHandlerTests
{
    private readonly ITransactionRepository _transactionRepo = Substitute.For<ITransactionRepository>();
    private readonly IPropositionRepository _propositionRepo = Substitute.For<IPropositionRepository>();

    private AnnulerTransactionCommandHandler CréerHandler() =>
        new(_transactionRepo, _propositionRepo);

    [Fact]
    public async Task AnnulerTransaction_EnCours_AnnuléeEtPropositionLibérée()
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
        await handler.Handle(new AnnulerTransactionCommand(transaction.Id), CancellationToken.None);

        // Assert
        Assert.Equal(StatutTransaction.Annulee, transaction.Statut);
        Assert.Equal(StatutProposition.Active, offre.Statut);
        await _transactionRepo.Received(1).UpdateAsync(transaction, Arg.Any<CancellationToken>());
        await _propositionRepo.Received(1).UpdateAsync(offre, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AnnulerTransaction_Introuvable_LèveNotFoundException()
    {
        // Arrange
        _transactionRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Transaction?)null);

        var handler = CréerHandler();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new AnnulerTransactionCommand(Guid.NewGuid()), CancellationToken.None).AsTask());
    }

    [Fact]
    public async Task AnnulerTransaction_PropositionNonEnTransaction_TransactionAnnuléeSansModifierProposition()
    {
        // Arrange — offre déjà active (non en transaction) : le handler ne doit pas appeler LibererDeTransaction
        var offre = new Offre("Titre", "Desc", Guid.NewGuid()); // statut Active par défaut

        var transaction = new Transaction(offre.Id, Guid.NewGuid());
        _transactionRepo.GetByIdAsync(transaction.Id, Arg.Any<CancellationToken>())
            .Returns(transaction);
        _propositionRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>())
            .Returns(offre);

        var handler = CréerHandler();

        // Act
        await handler.Handle(new AnnulerTransactionCommand(transaction.Id), CancellationToken.None);

        // Assert
        Assert.Equal(StatutTransaction.Annulee, transaction.Statut);
        Assert.Equal(StatutProposition.Active, offre.Statut); // inchangé
        await _transactionRepo.Received(1).UpdateAsync(transaction, Arg.Any<CancellationToken>());
        await _propositionRepo.DidNotReceive().UpdateAsync(Arg.Any<Proposition>(), Arg.Any<CancellationToken>());
    }
}
