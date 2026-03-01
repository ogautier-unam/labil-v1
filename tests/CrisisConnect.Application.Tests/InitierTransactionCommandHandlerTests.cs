using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Transactions.InitierTransaction;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class InitierTransactionCommandHandlerTests
{
    private readonly IPropositionRepository _propositionRepo = Substitute.For<IPropositionRepository>();
    private readonly ITransactionRepository _transactionRepo = Substitute.For<ITransactionRepository>();
    private readonly INotificationRepository _notificationRepo = Substitute.For<INotificationRepository>();
    private readonly INotificationService _notificationService = Substitute.For<INotificationService>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private InitierTransactionCommandHandler CréerHandler() =>
        new(_propositionRepo, _transactionRepo, _notificationRepo, _notificationService, _mapper);

    [Fact]
    public async Task InitierTransaction_OffreActive_TransactionCrééeEtRetournée()
    {
        // Arrange
        var offre = new Offre("Titre", "Desc", Guid.NewGuid());
        _propositionRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>())
            .Returns(offre);

        var command = new InitierTransactionCommand(offre.Id, Guid.NewGuid());
        var handler = CréerHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(offre.Id, result.PropositionId);
        Assert.Equal(StatutTransaction.EnCours, result.Statut);
        await _transactionRepo.Received(1).AddAsync(Arg.Any<Transaction>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task InitierTransaction_PropositionIntrouvable_LèveNotFoundException()
    {
        // Arrange
        _propositionRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Proposition?)null);

        var command = new InitierTransactionCommand(Guid.NewGuid(), Guid.NewGuid());
        var handler = CréerHandler();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None).AsTask());
        await _transactionRepo.DidNotReceive().AddAsync(Arg.Any<Transaction>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task InitierTransaction_OffreDéjàEnTransaction_LèveUneDomainException()
    {
        // Arrange
        var offre = new Offre("Titre", "Desc", Guid.NewGuid());
        offre.MarquerEnTransaction(); // offre déjà réservée
        _propositionRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>())
            .Returns(offre);

        var command = new InitierTransactionCommand(offre.Id, Guid.NewGuid());
        var handler = CréerHandler();

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None).AsTask());
        await _transactionRepo.DidNotReceive().AddAsync(Arg.Any<Transaction>(), Arg.Any<CancellationToken>());
    }
}
