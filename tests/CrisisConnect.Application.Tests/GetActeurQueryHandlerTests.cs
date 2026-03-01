using CrisisConnect.Application.UseCases.Acteurs.GetActeur;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetActeurQueryHandlerTests
{
    private readonly IPersonneRepository _personneRepo = Substitute.For<IPersonneRepository>();

    private GetActeurQueryHandler CréerHandler() => new(_personneRepo);

    [Fact]
    public async Task GetActeur_PersonneExistante_RetournePersonneDto()
    {
        // Arrange
        var personne = new Personne("alice@example.com", "hash", "Citoyen", "Alice", "Martin");
        _personneRepo.GetByIdAsync(personne.Id, Arg.Any<CancellationToken>()).Returns(personne);

        var query = new GetActeurQuery(personne.Id);

        // Act
        var result = await CréerHandler().Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(personne.Id, result.Id);
        Assert.Equal("alice@example.com", result.Email);
        Assert.Equal("Alice", result.Prenom);
        Assert.Equal("Martin", result.Nom);
        Assert.Equal("Alice Martin", result.NomComplet);
    }

    [Fact]
    public async Task GetActeur_PersonneIntrouvable_LèveNotFoundException()
    {
        // Arrange
        _personneRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Personne?)null);

        var query = new GetActeurQuery(Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => CréerHandler().Handle(query, CancellationToken.None).AsTask());
    }
}
