using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Tests;

public class EntiteTests
{
    private static Entite CréerEntite() =>
        new("asso@example.com", "hash", "Croix-Rouge locale", "Association de secours", "0600000000", Guid.NewGuid());

    [Fact]
    public void Entite_NouvelleInstance_EstActiveEtRoleEntite()
    {
        var entite = CréerEntite();

        Assert.True(entite.EstActive);
        Assert.Equal("Entite", entite.Role);
        Assert.Equal("Croix-Rouge locale", entite.Nom);
    }

    [Fact]
    public void Desactiver_EntiteActive_EstActiveFaux()
    {
        var entite = CréerEntite();

        entite.Desactiver();

        Assert.False(entite.EstActive);
        Assert.NotNull(entite.ModifieLe);
    }

    [Fact]
    public void Reactiver_EntiteInactive_EstActiveVrai()
    {
        var entite = CréerEntite();
        entite.Desactiver();

        entite.Reactiver();

        Assert.True(entite.EstActive);
    }
}
