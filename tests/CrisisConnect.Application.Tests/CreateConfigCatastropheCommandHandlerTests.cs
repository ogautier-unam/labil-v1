using AutoMapper;
using CrisisConnect.Application.UseCases.ConfigCatastrophe.CreateConfigCatastrophe;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class CreateConfigCatastropheCommandHandlerTests
{
    private readonly IConfigCatastropheRepository _configRepo = Substitute.For<IConfigCatastropheRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private CreateConfigCatastropheCommandHandler CréerHandler() => new(_configRepo, _mapper);

    [Fact]
    public async Task CreateConfigCatastrophe_ParamètresValides_RetourneDto()
    {
        // Arrange
        var command = new CreateConfigCatastropheCommand(
            Nom: "Inondation Var",
            Description: "Crise inondation 2026",
            ZoneGeographique: "PACA",
            EtatReferent: "France",
            DelaiArchivageJours: 30,
            DelaiRappelAvantArchivage: 7);

        // Act
        var result = await CréerHandler().Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Inondation Var", result.Nom);
        Assert.Equal("PACA", result.ZoneGeographique);
        Assert.True(result.EstActive);
        await _configRepo.Received(1).AddAsync(Arg.Any<ConfigCatastrophe>(), Arg.Any<CancellationToken>());
    }
}
