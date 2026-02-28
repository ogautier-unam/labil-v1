using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Tests;

public class RefreshTokenTests
{
    [Fact]
    public void EstValide_TokenNonRévoquéEtNonExpiré_RetourneVrai()
    {
        var token = new RefreshToken("tok-valide", Guid.NewGuid(), DateTime.UtcNow.AddDays(7));

        Assert.True(token.EstValide);
    }

    [Fact]
    public void EstValide_TokenExpiré_RetourneFaux()
    {
        var token = new RefreshToken("tok-expiré", Guid.NewGuid(), DateTime.UtcNow.AddDays(-1));

        Assert.False(token.EstValide);
    }

    [Fact]
    public void Revoquer_TokenValide_EstRévoquéEtInvalide()
    {
        var token = new RefreshToken("tok-révocable", Guid.NewGuid(), DateTime.UtcNow.AddDays(7));

        token.Revoquer();

        Assert.True(token.EstRevoque);
        Assert.False(token.EstValide);
    }

    [Fact]
    public void RefreshToken_NouvelleInstance_PropriétésInitialisées()
    {
        var personneId = Guid.NewGuid();
        var expiry = DateTime.UtcNow.AddDays(7);

        var token = new RefreshToken("my-token", personneId, expiry);

        Assert.Equal("my-token", token.Token);
        Assert.Equal(personneId, token.PersonneId);
        Assert.Equal(expiry, token.ExpiresAt);
        Assert.False(token.EstRevoque);
    }
}
