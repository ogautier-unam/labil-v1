using CrisisConnect.Application.UseCases.Acteurs.UpdateActeur;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class UpdateActeurCommandHandlerTests
{
    private readonly IPersonneRepository _personneRepo = Substitute.For<IPersonneRepository>();

    private UpdateActeurCommandHandler CréerHandler() => new(_personneRepo);

    [Fact]
    public async Task UpdateActeur_PersonneExistante_MiseAJourEtRetournée()
    {
        // Arrange
        var personne = new Personne("bob@example.com", "hash", "Benevole", "Robert", "Dupont");
        _personneRepo.GetByIdAsync(personne.Id, Arg.Any<CancellationToken>()).Returns(personne);

        var cmd = new UpdateActeurCommand(personne.Id, "Bobby", "Dupont",
            Telephone: "+33600000001", UrlPhoto: null, LanguePreferee: "fr",
            MoyensContact: null, AdresseRue: "1 rue de la Paix", AdresseVille: "Paris",
            AdresseCodePostal: "75001", AdressePays: "France");

        // Act
        var result = await CréerHandler().Handle(cmd, CancellationToken.None);

        // Assert
        Assert.Equal("Bobby", result.Prenom);
        Assert.Equal("Dupont", result.Nom);
        Assert.Equal("+33600000001", result.Telephone);
        Assert.Equal("1 rue de la Paix", result.AdresseRue);
        Assert.Equal("Paris", result.AdresseVille);
        await _personneRepo.Received(1).UpdateAsync(personne, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateActeur_PersonneIntrouvable_LèveNotFoundException()
    {
        // Arrange
        _personneRepo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Personne?)null);

        var cmd = new UpdateActeurCommand(Guid.NewGuid(), "Prénom", "Nom");

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => CréerHandler().Handle(cmd, CancellationToken.None).AsTask());
    }

    [Fact]
    public async Task UpdateActeur_SansAdresseComplète_AdresseNulle()
    {
        // Arrange
        var personne = new Personne("carol@example.com", "hash", "Citoyen", "Carol", "Blanc");
        _personneRepo.GetByIdAsync(personne.Id, Arg.Any<CancellationToken>()).Returns(personne);

        // Adresse incomplète : rue manquante
        var cmd = new UpdateActeurCommand(personne.Id, "Carol", "Blanc",
            AdresseVille: "Lyon", AdresseCodePostal: "69001");

        // Act
        var result = await CréerHandler().Handle(cmd, CancellationToken.None);

        // Assert — adresse non construite car rue absente
        Assert.Null(result.AdresseRue);
        Assert.Null(result.AdresseVille);
    }
}
