using AutoMapper;
using CrisisConnect.Application.UseCases.ConfigCatastrophe.UpdateConfigCatastrophe;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class UpdateConfigCatastropheCommandHandlerTests
{
    private readonly IConfigCatastropheRepository _configRepo = Substitute.For<IConfigCatastropheRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private UpdateConfigCatastropheCommandHandler CréerHandler() => new(_configRepo, _mapper);

    [Fact]
    public async Task UpdateConfig_ConfigExiste_ParamsMisAJourEtPersistée()
    {
        var config = new ConfigCatastrophe("Crise Test", "Description", "Belgique", "Actif", 30, 7);
        _configRepo.GetByIdAsync(config.Id, Arg.Any<CancellationToken>()).Returns(config);
        var command = new UpdateConfigCatastropheCommand(config.Id, 60, 14, true);

        var result = await CréerHandler().Handle(command, CancellationToken.None);

        Assert.Equal(60, result.DelaiArchivageJours);
        Assert.Equal(14, result.DelaiRappelAvantArchivage);
        Assert.True(result.EstActive);
        await _configRepo.Received(1).UpdateAsync(config, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateConfig_EstActivefalse_ConfigDesactivée()
    {
        var config = new ConfigCatastrophe("Crise Test", "Description", "Belgique", "Actif");
        _configRepo.GetByIdAsync(config.Id, Arg.Any<CancellationToken>()).Returns(config);
        var command = new UpdateConfigCatastropheCommand(config.Id, 30, 7, false);

        var result = await CréerHandler().Handle(command, CancellationToken.None);

        Assert.False(result.EstActive);
    }

    [Fact]
    public async Task UpdateConfig_ConfigIntrouvable_LèveNotFoundException()
    {
        var id = Guid.NewGuid();
        _configRepo.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((ConfigCatastrophe?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new UpdateConfigCatastropheCommand(id, 30, 7, true), CancellationToken.None));
    }
}
