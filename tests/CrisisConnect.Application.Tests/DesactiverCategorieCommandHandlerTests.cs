using CrisisConnect.Application.UseCases.Taxonomie.DesactiverCategorie;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class DesactiverCategorieCommandHandlerTests
{
    private readonly ICategorieTaxonomieRepository _categorieRepo = Substitute.For<ICategorieTaxonomieRepository>();

    private DesactiverCategorieCommandHandler CréerHandler() => new(_categorieRepo);

    [Fact]
    public async Task DesactiverCategorie_CategorieExiste_DesactivéeEtPersistée()
    {
        var categorie = new CategorieTaxonomie("LOGEMENT", "{\"fr\":\"Logement\"}", Guid.NewGuid());
        Assert.True(categorie.EstActive);
        _categorieRepo.GetByIdAsync(categorie.Id, Arg.Any<CancellationToken>()).Returns(categorie);

        await CréerHandler().Handle(new DesactiverCategorieCommand(categorie.Id), CancellationToken.None);

        Assert.False(categorie.EstActive);
        await _categorieRepo.Received(1).UpdateAsync(categorie, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DesactiverCategorie_CategorieIntrouvable_LèveNotFoundException()
    {
        var id = Guid.NewGuid();
        _categorieRepo.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((CategorieTaxonomie?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new DesactiverCategorieCommand(id), CancellationToken.None));
    }
}
