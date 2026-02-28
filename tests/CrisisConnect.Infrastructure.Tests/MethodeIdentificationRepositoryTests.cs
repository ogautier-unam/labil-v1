using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class MethodeIdentificationRepositoryTests
{
    [Fact]
    public async Task AddAsync_LoginPassword_PuisGetByIdAsync_RetourneMethode()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new MethodeIdentificationRepository(ctx);

        var personneId = Guid.NewGuid();
        var methode = new LoginPassword(personneId, "alice@example.com", "hash_mdp");
        await repo.AddAsync(methode);

        var résultat = await repo.GetByIdAsync(methode.Id);

        Assert.NotNull(résultat);
        Assert.IsType<LoginPassword>(résultat);
        Assert.Equal(personneId, résultat.PersonneId);
        Assert.True(résultat.EstVerifiee); // LoginPassword est vérifiée dès la création
    }

    [Fact]
    public async Task GetByPersonneAsync_FiltreParPersonne()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new MethodeIdentificationRepository(ctx);

        var personne1 = Guid.NewGuid();
        var personne2 = Guid.NewGuid();

        await repo.AddAsync(new LoginPassword(personne1, "p1@example.com", "hash1"));
        await repo.AddAsync(new VerificationSMS(personne1, "0600000001"));
        await repo.AddAsync(new LoginPassword(personne2, "p2@example.com", "hash2"));

        var résultats = await repo.GetByPersonneAsync(personne1);

        Assert.Equal(2, résultats.Count);
        Assert.All(résultats, m => Assert.Equal(personne1, m.PersonneId));
    }

    [Fact]
    public async Task UpdateAsync_MarquerVerifiee_PersistéEnBase()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new MethodeIdentificationRepository(ctx);

        var personneId = Guid.NewGuid();
        var methode = new VerificationSMS(personneId, "0699000000");
        await repo.AddAsync(methode);

        Assert.False(methode.EstVerifiee);
        methode.MarquerVerifiee();
        await repo.UpdateAsync(methode);

        var chargée = await repo.GetByIdAsync(methode.Id);
        Assert.True(chargée!.EstVerifiee);
    }
}
