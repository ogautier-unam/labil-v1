using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Taxonomie.GetCategories;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetCategoriesQueryHandlerTests
{
    private readonly ICategorieTaxonomieRepository _categorieRepo = Substitute.For<ICategorieTaxonomieRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetCategoriesQueryHandler CréerHandler() => new(_categorieRepo, _mapper);

    [Fact]
    public async Task GetCategories_ConfigAvecCatégories_RetourneListe()
    {
        // Arrange
        var configId = Guid.NewGuid();
        var categories = new List<CategorieTaxonomie>
        {
            new("LOGEMENT", "{\"fr\":\"Logement\"}", configId),
            new("SOINS", "{\"fr\":\"Soins\"}", configId)
        };
        _categorieRepo.GetRacinesAsync(configId, Arg.Any<CancellationToken>())
            .Returns(categories.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetCategoriesQuery(configId), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Code == "LOGEMENT");
        Assert.Contains(result, c => c.Code == "SOINS");
    }

    [Fact]
    public async Task GetCategories_AucuneCatégorie_RetourneListeVide()
    {
        // Arrange
        var configId = Guid.NewGuid();
        _categorieRepo.GetRacinesAsync(configId, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<CategorieTaxonomie>());

        // Act
        var result = await CréerHandler().Handle(new GetCategoriesQuery(configId), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
