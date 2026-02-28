using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Tests;

public class PersonneTests
{
    [Fact]
    public void NouvelleInstance_NomComplet_CombinePrenonEtNom()
    {
        var p = new Personne("alice@example.com", "hash", "Citoyen", "Alice", "Dupont");

        Assert.Equal("Alice Dupont", p.NomComplet);
    }

    [Fact]
    public void NouvelleInstance_EmailEtRoleSetCorrectement()
    {
        var p = new Personne("bob@example.com", "hash", "Benevole", "Bob", "Martin");

        Assert.Equal("bob@example.com", p.Email);
        Assert.Equal("Benevole", p.Role);
    }

    [Fact]
    public void NouvelleInstance_PrenomEtNomSetCorrectement()
    {
        var p = new Personne("carol@example.com", "hash", "Coordinateur", "Carol", "Leclerc");

        Assert.Equal("Carol", p.Prenom);
        Assert.Equal("Leclerc", p.Nom);
    }
}
