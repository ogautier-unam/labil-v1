using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Tests;

public class SuggestionAppariementTests
{
    [Fact]
    public void NouvelleInstance_EstPasAcknowledged()
    {
        var s = new SuggestionAppariement(Guid.NewGuid(), Guid.NewGuid(), 0.75, "Correspondance géo");

        Assert.False(s.EstAcknowledged);
        Assert.Equal(0.75, s.ScoreCorrespondance);
    }

    [Fact]
    public void Acknowledger_MarqueEstAcknowledged()
    {
        var s = new SuggestionAppariement(Guid.NewGuid(), Guid.NewGuid(), 0.9, "Correspondance catégorie");

        s.Acknowledger();

        Assert.True(s.EstAcknowledged);
    }

    [Fact]
    public void NouvelleInstance_OffreIdEtDemandeId_SetCorrectement()
    {
        var offreId = Guid.NewGuid();
        var demandeId = Guid.NewGuid();

        var s = new SuggestionAppariement(offreId, demandeId, 0.5, "Test");

        Assert.Equal(offreId, s.OffreId);
        Assert.Equal(demandeId, s.DemandeId);
        Assert.Equal("Test", s.Raisonnement);
    }
}
