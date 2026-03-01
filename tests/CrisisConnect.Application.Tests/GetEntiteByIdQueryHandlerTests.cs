using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Entites.GetEntiteById;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetEntiteByIdQueryHandlerTests
{
    private readonly IEntiteRepository _entiteRepo = Substitute.For<IEntiteRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetEntiteByIdQueryHandler CréerHandler() => new(_entiteRepo, _mapper);

    [Fact]
    public async Task GetEntiteById_EntiteExiste_RetourneDto()
    {
        // Arrange
        var entite = new Entite("contact@ong.org", "hash", "ONG Secours", "Description", "Tel", Guid.NewGuid());
        _entiteRepo.GetByIdAsync(entite.Id, Arg.Any<CancellationToken>()).Returns(entite);

        // Act
        var result = await CréerHandler().Handle(new GetEntiteByIdQuery(entite.Id), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entite.Id, result.Id);
        Assert.Equal("ONG Secours", result.Nom);
    }

    [Fact]
    public async Task GetEntiteById_EntiteIntrouvable_LèveNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _entiteRepo.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Entite?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => CréerHandler().Handle(new GetEntiteByIdQuery(id), CancellationToken.None).AsTask());
    }
}
