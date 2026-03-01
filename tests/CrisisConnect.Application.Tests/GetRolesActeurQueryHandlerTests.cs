using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Roles.GetRolesActeur;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetRolesActeurQueryHandlerTests
{
    private readonly IAttributionRoleRepository _roleRepo = Substitute.For<IAttributionRoleRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetRolesActeurQueryHandler CréerHandler() => new(_roleRepo, _mapper);

    [Fact]
    public async Task GetRolesActeur_ActeurAvecRôles_RetourneListe()
    {
        // Arrange
        var acteurId = Guid.NewGuid();
        var roles = new List<AttributionRole>
        {
            new(acteurId, TypeRole.Contributeur, DateTime.Today),
            new(acteurId, TypeRole.AdminCatastrophe, DateTime.Today, DateTime.Today.AddMonths(6))
        };
        _roleRepo.GetByActeurAsync(acteurId, Arg.Any<CancellationToken>())
            .Returns(roles.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetRolesActeurQuery(acteurId), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.TypeRole == TypeRole.Contributeur.ToString());
        Assert.Contains(result, r => r.TypeRole == TypeRole.AdminCatastrophe.ToString());
    }

    [Fact]
    public async Task GetRolesActeur_AucunRôle_RetourneListeVide()
    {
        // Arrange
        var acteurId = Guid.NewGuid();
        _roleRepo.GetByActeurAsync(acteurId, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<AttributionRole>());

        // Act
        var result = await CréerHandler().Handle(new GetRolesActeurQuery(acteurId), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
