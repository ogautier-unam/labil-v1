using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Application.UseCases.Paniers.CreatePanier;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class CreatePanierCommandHandlerTests
{
    private readonly IPanierRepository _panierRepo = Substitute.For<IPanierRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private CreatePanierCommandHandler CréerHandler() =>
        new(_panierRepo, _mapper);

    [Fact]
    public async Task CreatePanier_AucunPanierOuvert_CrééEtRetourné()
    {
        // Arrange
        var proprietaireId = Guid.NewGuid();
        _panierRepo.GetByProprietaireAsync(proprietaireId, Arg.Any<CancellationToken>())
            .Returns([]);

        var handler = CréerHandler();
        var command = new CreatePanierCommand(proprietaireId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(proprietaireId, result.ProprietaireId);
        Assert.Equal(nameof(StatutPanier.Ouvert), result.Statut);
        await _panierRepo.Received(1).AddAsync(Arg.Any<Panier>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreatePanier_UnPanierOuvertExiste_LèveUneDomainException()
    {
        // Arrange
        var proprietaireId = Guid.NewGuid();
        var panierExistant = new Panier(proprietaireId);
        _panierRepo.GetByProprietaireAsync(proprietaireId, Arg.Any<CancellationToken>())
            .Returns([panierExistant]);

        var handler = CréerHandler();
        var command = new CreatePanierCommand(proprietaireId);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));
        await _panierRepo.DidNotReceive().AddAsync(Arg.Any<Panier>(), Arg.Any<CancellationToken>());
    }
}
