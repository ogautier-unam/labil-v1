using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Propositions.CreatePropositionAvecValidation;
using CrisisConnect.Application.UseCases.Propositions.RefuserValidationProposition;
using CrisisConnect.Application.UseCases.Propositions.ValiderProposition;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class PropositionAvecValidationCommandHandlerTests
{
    private readonly IPropositionRepository _propositionRepo = Substitute.For<IPropositionRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    // ── CreatePropositionAvecValidation ────────────────────────────────────────

    [Fact]
    public async Task Create_PropositionAvecValidation_CrééeEnAttenteEtRetournée()
    {
        // Arrange
        var cmd = new CreatePropositionAvecValidationCommand(
            "Aide psychologique", "Accompagnement victimes", Guid.NewGuid(),
            "Validée par Centre Psycho agréé");

        // Act
        var result = await new CreatePropositionAvecValidationCommandHandler(_propositionRepo, _mapper)
            .Handle(cmd, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Aide psychologique", result.Titre);
        Assert.Equal(StatutValidation.EnAttente, result.StatutValidation);
        // Non visible tant que non validée (statut = Archivee)
        Assert.Equal(StatutProposition.Archivee, result.Statut);
        await _propositionRepo.Received(1).AddAsync(Arg.Any<Proposition>(), Arg.Any<CancellationToken>());
    }

    // ── ValiderProposition ────────────────────────────────────────────────────

    [Fact]
    public async Task Valider_PropositionEnAttente_StatutActiveEtValidee()
    {
        // Arrange
        var prop = new PropositionAvecValidation("Aide psy", "Desc", Guid.NewGuid(), "Validation requise");
        _propositionRepo.GetByIdAsync(prop.Id, Arg.Any<CancellationToken>()).Returns(prop);

        var entiteId = Guid.NewGuid();
        var cmd = new ValiderPropositionCommand(prop.Id, entiteId);

        // Act
        var result = await new ValiderPropositionCommandHandler(_propositionRepo, _mapper)
            .Handle(cmd, CancellationToken.None);

        // Assert
        Assert.Equal(StatutValidation.Validee, result.StatutValidation);
        Assert.Equal(StatutProposition.Active, result.Statut);
        Assert.Equal(entiteId, result.ValideurEntiteId);
        await _propositionRepo.Received(1).UpdateAsync(prop, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Valider_PropositionIntrouvable_LèveNotFoundException()
    {
        // Arrange
        _propositionRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Proposition?)null);

        var cmd = new ValiderPropositionCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => new ValiderPropositionCommandHandler(_propositionRepo, _mapper)
                .Handle(cmd, CancellationToken.None).AsTask());
    }

    // ── RefuserValidationProposition ──────────────────────────────────────────

    [Fact]
    public async Task RefuserValidation_PropositionEnAttente_StatutRefusee()
    {
        // Arrange
        var prop = new PropositionAvecValidation("Aide psy", "Desc", Guid.NewGuid(), "Validation requise");
        _propositionRepo.GetByIdAsync(prop.Id, Arg.Any<CancellationToken>()).Returns(prop);

        var entiteId = Guid.NewGuid();
        var cmd = new RefuserValidationPropositionCommand(prop.Id, entiteId);

        // Act
        var result = await new RefuserValidationPropositionCommandHandler(_propositionRepo, _mapper)
            .Handle(cmd, CancellationToken.None);

        // Assert
        Assert.Equal(StatutValidation.Refusee, result.StatutValidation);
        Assert.Equal(entiteId, result.ValideurEntiteId);
        await _propositionRepo.Received(1).UpdateAsync(prop, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RefuserValidation_PropositionIntrouvable_LèveNotFoundException()
    {
        // Arrange
        _propositionRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Proposition?)null);

        var cmd = new RefuserValidationPropositionCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => new RefuserValidationPropositionCommandHandler(_propositionRepo, _mapper)
                .Handle(cmd, CancellationToken.None).AsTask());
    }
}
