using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Roles.AttribuerRole;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class AttribuerRoleCommandHandlerTests
{
    private readonly IAttributionRoleRepository _roleRepo = Substitute.For<IAttributionRoleRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private AttribuerRoleCommandHandler CréerHandler() => new(_roleRepo, _mapper);

    [Fact]
    public async Task AttribuerRole_SansDates_CréeAttributionRetournéeDto()
    {
        var acteurId = Guid.NewGuid();
        var command = new AttribuerRoleCommand(acteurId, TypeRole.Contributeur, DateTime.Today);

        var result = await CréerHandler().Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(acteurId, result.ActeurId);
        await _roleRepo.Received(1).AddAsync(Arg.Any<AttributionRole>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AttribuerRole_AvecDateFin_DateFinPersistée()
    {
        var acteurId = Guid.NewGuid();
        var dateFin = DateTime.Today.AddMonths(6);
        var command = new AttribuerRoleCommand(acteurId, TypeRole.AdminCatastrophe, DateTime.Today, dateFin);

        var result = await CréerHandler().Handle(command, CancellationToken.None);

        Assert.Equal(dateFin, result.DateFin);
        await _roleRepo.Received(1).AddAsync(Arg.Any<AttributionRole>(), Arg.Any<CancellationToken>());
    }
}
