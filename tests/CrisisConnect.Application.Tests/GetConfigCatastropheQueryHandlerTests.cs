using AutoMapper;
using CrisisConnect.Application.UseCases.ConfigCatastrophe.GetConfigCatastrophe;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetConfigCatastropheQueryHandlerTests
{
    private readonly IConfigCatastropheRepository _configRepo = Substitute.For<IConfigCatastropheRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private GetConfigCatastropheQueryHandler CréerHandler() => new(_configRepo, _mapper);

    [Fact]
    public async Task GetConfigCatastrophe_ConfigActive_RetourneDto()
    {
        // Arrange
        var config = new ConfigCatastrophe("Séisme", "Crise sismique", "Méditerranée", "France");
        _configRepo.GetActiveAsync(Arg.Any<CancellationToken>())
            .Returns(config);

        // Act
        var result = await CréerHandler().Handle(new GetConfigCatastropheQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Séisme", result!.Nom);
        Assert.True(result.EstActive);
    }

    [Fact]
    public async Task GetConfigCatastrophe_AucuneConfigActive_RetourneNull()
    {
        // Arrange
        _configRepo.GetActiveAsync(Arg.Any<CancellationToken>())
            .Returns((ConfigCatastrophe?)null);

        // Act
        var result = await CréerHandler().Handle(new GetConfigCatastropheQuery(), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
