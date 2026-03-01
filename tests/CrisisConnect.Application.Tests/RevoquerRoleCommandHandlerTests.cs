using CrisisConnect.Application.UseCases.Roles.RevoquerRole;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class RevoquerRoleCommandHandlerTests
{
    private readonly IAttributionRoleRepository _roleRepo = Substitute.For<IAttributionRoleRepository>();

    private RevoquerRoleCommandHandler CréerHandler() => new(_roleRepo);

    [Fact]
    public async Task RevoquerRole_AttributionExiste_StatutExpirEtPersisté()
    {
        var attribution = new AttributionRole(Guid.NewGuid(), TypeRole.Contributeur, DateTime.Today);
        _roleRepo.GetByIdAsync(attribution.Id, Arg.Any<CancellationToken>()).Returns(attribution);

        await CréerHandler().Handle(new RevoquerRoleCommand(attribution.Id), CancellationToken.None);

        Assert.Equal(StatutRole.Expire, attribution.Statut);
        await _roleRepo.Received(1).UpdateAsync(attribution, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RevoquerRole_AttributionIntrouvable_LèveNotFoundException()
    {
        var id = Guid.NewGuid();
        _roleRepo.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((AttributionRole?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new RevoquerRoleCommand(id), CancellationToken.None));
    }
}
