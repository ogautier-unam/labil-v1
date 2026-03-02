using CrisisConnect.Application.UseCases.Taxonomie.CreateCategorie;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class CreateCategorieCommandHandlerTests
{
    private readonly ICategorieTaxonomieRepository _categorieRepo = Substitute.For<ICategorieTaxonomieRepository>();

    private CreateCategorieCommandHandler CréerHandler() => new(_categorieRepo);

    [Fact]
    public async Task CreateCategorie_SansParent_CatégorieRacineCréée()
    {
        var configId = Guid.NewGuid();
        var command = new CreateCategorieCommand("LOGEMENT", "{\"fr\":\"Logement\"}", configId);

        var result = await CréerHandler().Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("LOGEMENT", result.Code);
        Assert.Null(result.ParentId);
        Assert.True(result.EstActive);
        await _categorieRepo.Received(1).AddAsync(Arg.Any<CategorieTaxonomie>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateCategorie_AvecParent_ParentIdPersisté()
    {
        var configId = Guid.NewGuid();
        var parentId = Guid.NewGuid();
        var command = new CreateCategorieCommand("LOGEMENT_URGENCE", "{\"fr\":\"Logement urgence\"}", configId, parentId);

        var result = await CréerHandler().Handle(command, CancellationToken.None);

        Assert.Equal(parentId, result.ParentId);
    }
}
