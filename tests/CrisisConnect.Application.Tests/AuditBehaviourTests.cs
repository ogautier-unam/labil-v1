using CrisisConnect.Application.Common.Behaviours;
using CrisisConnect.Application.Common.Interfaces;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

// Types publics pour éviter les problèmes de proxy NSubstitute avec ILogger<T> générique
public record AuditTestCommand : IRequest<string>;
public record AuditTestQuery : IRequest<string>;

public class AuditBehaviourTests
{
    private readonly ICurrentUserService _currentUser = Substitute.For<ICurrentUserService>();
    private readonly IEntreeJournalRepository _journalRepo = Substitute.For<IEntreeJournalRepository>();

    private AuditBehaviour<AuditTestCommand, string> CréerCommandBehaviour() =>
        new(_currentUser, _journalRepo, NullLogger<AuditBehaviour<AuditTestCommand, string>>.Instance);

    private AuditBehaviour<AuditTestQuery, string> CréerQueryBehaviour() =>
        new(_currentUser, _journalRepo, NullLogger<AuditBehaviour<AuditTestQuery, string>>.Instance);

    [Fact]
    public async Task Handle_RequêteQuery_NePersistePasJournal()
    {
        // Arrange
        RequestHandlerDelegate<string> next = _ => Task.FromResult("ok");

        // Act
        var result = await CréerQueryBehaviour().Handle(new AuditTestQuery(), next, CancellationToken.None);

        // Assert
        Assert.Equal("ok", result);
        await _journalRepo.DidNotReceive().AddAsync(Arg.Any<EntreeJournal>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Commande_PersisteLEntreeJournal()
    {
        // Arrange
        _currentUser.UserId.Returns(Guid.NewGuid());
        RequestHandlerDelegate<string> next = _ => Task.FromResult("ok");

        // Act
        var result = await CréerCommandBehaviour().Handle(new AuditTestCommand(), next, CancellationToken.None);

        // Assert
        Assert.Equal("ok", result);
        await _journalRepo.Received(1).AddAsync(Arg.Any<EntreeJournal>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_JournalLèveException_ResponseToujoursRetournée()
    {
        // Arrange — l'audit ne doit JAMAIS bloquer la commande principale
        _currentUser.UserId.Returns(Guid.NewGuid());
        _journalRepo.AddAsync(Arg.Any<EntreeJournal>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(new InvalidOperationException("DB indisponible")));
        RequestHandlerDelegate<string> next = _ => Task.FromResult("ok");

        // Act — ne doit pas lever d'exception
        var result = await CréerCommandBehaviour().Handle(new AuditTestCommand(), next, CancellationToken.None);

        // Assert
        Assert.Equal("ok", result);
    }

    [Fact]
    public async Task Handle_Commande_TransmetLaRequêteAuHandlerSuivant()
    {
        // Arrange
        _currentUser.UserId.Returns(Guid.NewGuid());
        var nextCalled = false;
        RequestHandlerDelegate<string> next = _ =>
        {
            nextCalled = true;
            return Task.FromResult("handled");
        };

        // Act
        await CréerCommandBehaviour().Handle(new AuditTestCommand(), next, CancellationToken.None);

        // Assert — next() a bien été appelé avant la persistance
        Assert.True(nextCalled);
    }
}
