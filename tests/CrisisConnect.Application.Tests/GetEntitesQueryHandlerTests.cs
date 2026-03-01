using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Entites.GetEntites;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetEntitesQueryHandlerTests
{
    private readonly IEntiteRepository _entiteRepo = Substitute.For<IEntiteRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetEntitesQueryHandler CréerHandler() => new(_entiteRepo, _mapper);

    [Fact]
    public async Task GetEntites_EntitesExistantes_RetourneListe()
    {
        // Arrange
        var entites = new List<Entite>
        {
            new("contact@ong.be", "hash1", "Croix-Rouge", "Org humanitaire", "Tél: +32 2", Guid.NewGuid()),
            new("info@pompiers.be", "hash2", "Pompiers", "Service incendie", "Tél: 100", Guid.NewGuid())
        };
        _entiteRepo.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(entites.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetEntitesQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, e => e.Nom == "Croix-Rouge");
        Assert.Contains(result, e => e.Nom == "Pompiers");
    }

    [Fact]
    public async Task GetEntites_AucuneEntité_RetourneListeVide()
    {
        // Arrange
        _entiteRepo.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Entite>());

        // Act
        var result = await CréerHandler().Handle(new GetEntitesQuery(), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
