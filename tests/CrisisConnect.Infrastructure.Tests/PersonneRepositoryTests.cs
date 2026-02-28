using CrisisConnect.Domain.Entities;
using CrisisConnect.Infrastructure.Persistence.Repositories;

namespace CrisisConnect.Infrastructure.Tests;

public class PersonneRepositoryTests
{
    [Fact]
    public async Task AddAsync_PuisGetByIdAsync_RetournePersonne()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new PersonneRepository(ctx);

        var personne = new Personne("alice@example.com", "hash_mdp", "Citoyen", "Alice", "Dupont");
        await repo.AddAsync(personne);

        var résultat = await repo.GetByIdAsync(personne.Id);

        Assert.NotNull(résultat);
        Assert.Equal("alice@example.com", résultat.Email);
        Assert.Equal("Alice Dupont", résultat.NomComplet);
    }

    [Fact]
    public async Task GetByEmailAsync_EmailExistant_RetournePersonne()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new PersonneRepository(ctx);

        var personne = new Personne("bob@example.com", "hash_mdp", "Benevole", "Bob", "Martin");
        await repo.AddAsync(personne);

        var résultat = await repo.GetByEmailAsync("bob@example.com");

        Assert.NotNull(résultat);
        Assert.Equal(personne.Id, résultat.Id);
    }

    [Fact]
    public async Task GetByEmailAsync_EmailInexistant_RetourneNull()
    {
        await using var ctx = DbContextFactory.Créer();
        var repo = new PersonneRepository(ctx);

        var résultat = await repo.GetByEmailAsync("inconnu@example.com");

        Assert.Null(résultat);
    }
}
