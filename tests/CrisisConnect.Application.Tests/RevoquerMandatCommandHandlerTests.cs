using CrisisConnect.Application.UseCases.Mandats.RevoquerMandat;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class RevoquerMandatCommandHandlerTests
{
    private readonly IMandatRepository _mandatRepo = Substitute.For<IMandatRepository>();

    private RevoquerMandatCommandHandler CréerHandler() => new(_mandatRepo);

    [Fact]
    public async Task RevoquerMandat_MandatExiste_DateFinFixéeEtPersisté()
    {
        var mandat = new Mandat(Guid.NewGuid(), Guid.NewGuid(), PorteeMandat.LectureSeule,
            "Description", false, DateTime.Today);
        Assert.True(mandat.EstActif);
        _mandatRepo.GetByIdAsync(mandat.Id, Arg.Any<CancellationToken>()).Returns(mandat);

        await CréerHandler().Handle(new RevoquerMandatCommand(mandat.Id), CancellationToken.None);

        Assert.False(mandat.EstActif);
        await _mandatRepo.Received(1).UpdateAsync(mandat, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RevoquerMandat_MandatIntrouvable_LèveNotFoundException()
    {
        var id = Guid.NewGuid();
        _mandatRepo.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Mandat?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new RevoquerMandatCommand(id), CancellationToken.None));
    }
}
