using CrisisConnect.Application.UseCases.Propositions.ArchiverProposition;
using CrisisConnect.Application.UseCases.Propositions.CloreProposition;
using CrisisConnect.Application.UseCases.Propositions.MarquerEnAttenteRelance;
using CrisisConnect.Application.UseCases.Propositions.ReconfirmerProposition;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class PropositionLifecycleCommandHandlerTests
{
    private readonly IPropositionRepository _propositionRepo = Substitute.For<IPropositionRepository>();

    private static Offre CréerOffre() => new("Titre", "Description", Guid.NewGuid());

    // ── ArchiverProposition ───────────────────────────────────────────────────

    [Fact]
    public async Task Archiver_PropositionExiste_ArchivedEtPersistée()
    {
        var offre = CréerOffre();
        _propositionRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>()).Returns(offre);

        await new ArchiverPropositionCommandHandler(_propositionRepo)
            .Handle(new ArchiverPropositionCommand(offre.Id), CancellationToken.None);

        Assert.Equal(Domain.Enums.StatutProposition.Archivee, offre.Statut);
        await _propositionRepo.Received(1).UpdateAsync(offre, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Archiver_PropositionIntrouvable_LèveNotFoundException()
    {
        var id = Guid.NewGuid();
        _propositionRepo.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Proposition?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            new ArchiverPropositionCommandHandler(_propositionRepo)
                .Handle(new ArchiverPropositionCommand(id), CancellationToken.None));
    }

    // ── CloreProposition ──────────────────────────────────────────────────────

    [Fact]
    public async Task Clore_PropositionExiste_ClorEtPersistée()
    {
        var offre = CréerOffre();
        _propositionRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>()).Returns(offre);

        await new ClorePropositionCommandHandler(_propositionRepo)
            .Handle(new ClorePropositionCommand(offre.Id), CancellationToken.None);

        Assert.Equal(Domain.Enums.StatutProposition.Cloturee, offre.Statut);
        await _propositionRepo.Received(1).UpdateAsync(offre, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Clore_PropositionIntrouvable_LèveNotFoundException()
    {
        var id = Guid.NewGuid();
        _propositionRepo.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Proposition?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            new ClorePropositionCommandHandler(_propositionRepo)
                .Handle(new ClorePropositionCommand(id), CancellationToken.None));
    }

    // ── MarquerEnAttenteRelance ───────────────────────────────────────────────

    [Fact]
    public async Task MarquerRelance_PropositionExiste_StatutMisAJourEtPersisté()
    {
        var offre = CréerOffre();
        _propositionRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>()).Returns(offre);

        await new MarquerEnAttenteRelanceCommandHandler(_propositionRepo)
            .Handle(new MarquerEnAttenteRelanceCommand(offre.Id), CancellationToken.None);

        Assert.Equal(Domain.Enums.StatutProposition.EnAttenteRelance, offre.Statut);
        await _propositionRepo.Received(1).UpdateAsync(offre, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task MarquerRelance_PropositionIntrouvable_LèveNotFoundException()
    {
        var id = Guid.NewGuid();
        _propositionRepo.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Proposition?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            new MarquerEnAttenteRelanceCommandHandler(_propositionRepo)
                .Handle(new MarquerEnAttenteRelanceCommand(id), CancellationToken.None));
    }

    // ── ReconfirmerProposition ────────────────────────────────────────────────

    [Fact]
    public async Task Reconfirmer_PropositionEnAttenteRelance_RedevientActive()
    {
        var offre = CréerOffre();
        offre.MarquerEnAttenteRelance(); // EnAttenteRelance avant de reconfirmer
        _propositionRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>()).Returns(offre);

        await new ReconfirmerPropositionCommandHandler(_propositionRepo)
            .Handle(new ReconfirmerPropositionCommand(offre.Id), CancellationToken.None);

        Assert.Equal(Domain.Enums.StatutProposition.Active, offre.Statut);
        await _propositionRepo.Received(1).UpdateAsync(offre, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Reconfirmer_PropositionIntrouvable_LèveNotFoundException()
    {
        var id = Guid.NewGuid();
        _propositionRepo.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Proposition?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            new ReconfirmerPropositionCommandHandler(_propositionRepo)
                .Handle(new ReconfirmerPropositionCommand(id), CancellationToken.None));
    }
}
