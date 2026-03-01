using AutoMapper;
using CrisisConnect.Application.UseCases.Mandats.CreerMandat;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class CreerMandatCommandHandlerTests
{
    private readonly IMandatRepository _mandatRepo = Substitute.For<IMandatRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private CreerMandatCommandHandler CréerHandler() => new(_mandatRepo, _mapper);

    [Fact]
    public async Task CréerMandat_SansDates_MandatCréeEtRetournéDto()
    {
        var mandantId = Guid.NewGuid();
        var mandataireId = Guid.NewGuid();
        var command = new CreerMandatCommand(mandantId, mandataireId, PorteeMandat.LectureSeule,
            "Délégation logistique", false, DateTime.Today);

        var result = await CréerHandler().Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(mandantId, result.MandantId);
        Assert.Equal(mandataireId, result.MandataireId);
        Assert.True(result.EstActif);
        await _mandatRepo.Received(1).AddAsync(Arg.Any<Mandat>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CréerMandat_AvecDateFin_DateFinPersistée()
    {
        var dateFin = DateTime.Today.AddMonths(3);
        var command = new CreerMandatCommand(Guid.NewGuid(), Guid.NewGuid(), PorteeMandat.ToutesOperations,
            "Mandat temporaire", true, DateTime.Today, dateFin);

        var result = await CréerHandler().Handle(command, CancellationToken.None);

        Assert.Equal(dateFin, result.DateFin);
    }
}
