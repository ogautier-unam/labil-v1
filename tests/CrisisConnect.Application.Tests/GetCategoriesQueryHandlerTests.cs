using CrisisConnect.Application.UseCases.Taxonomie.GetCategories;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetCategoriesQueryHandlerTests
{
    private readonly ICategorieTaxonomieRepository _categorieRepo = Substitute.For<ICategorieTaxonomieRepository>();

    private GetCategoriesQueryHandler CréerHandler() => new(_categorieRepo);

    [Fact]
    public async Task GetCategories_ConfigAvecCatégories_RetourneListe()
    {
        // Arrange
        var configId = Guid.NewGuid();
        var categories = new List<CategorieTaxonomie>
        {
            new("LOGEMENT", "{\"fr\":\"Logement\",\"en\":\"Housing\"}", configId),
            new("SOINS", "{\"fr\":\"Soins\",\"en\":\"Care\"}", configId)
        };
        _categorieRepo.GetRacinesAsync(configId, Arg.Any<CancellationToken>())
            .Returns(categories.AsReadOnly());

        // Act — langue par défaut "fr"
        var result = await CréerHandler().Handle(new GetCategoriesQuery(configId), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Code == "LOGEMENT" && c.Nom == "Logement");
        Assert.Contains(result, c => c.Code == "SOINS" && c.Nom == "Soins");
    }

    [Fact]
    public async Task GetCategories_LangueAnglais_RetourneNomLocalisé()
    {
        // Arrange
        var configId = Guid.NewGuid();
        var categories = new List<CategorieTaxonomie>
        {
            new("LOGEMENT", "{\"fr\":\"Logement\",\"en\":\"Housing\"}", configId)
        };
        _categorieRepo.GetRacinesAsync(configId, Arg.Any<CancellationToken>())
            .Returns(categories.AsReadOnly());

        // Act — langue "en"
        var result = await CréerHandler().Handle(new GetCategoriesQuery(configId, "en"), CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal("Housing", result[0].Nom);
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
