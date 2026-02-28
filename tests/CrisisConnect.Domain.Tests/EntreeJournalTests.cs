using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Tests;

public class EntreeJournalTests
{
    [Fact]
    public void NouvelleInstance_ActeurIdEtTypeOperation_Setés()
    {
        var acteurId = Guid.NewGuid();
        var e = new EntreeJournal(acteurId, TypeOperation.Connexion);

        Assert.Equal(acteurId, e.ActeurId);
        Assert.Equal(TypeOperation.Connexion, e.TypeOperation);
    }

    [Fact]
    public void NouvelleInstance_AvecDetails_ChampsPersistés()
    {
        var acteurId = Guid.NewGuid();
        var cibleId = Guid.NewGuid();
        var e = new EntreeJournal(acteurId, TypeOperation.DepotProposition,
            entiteCibleId: cibleId,
            entiteCibleType: "Offre",
            details: "Offre créée",
            adresseIP: "127.0.0.1",
            sessionId: "sess-001");

        Assert.Equal(cibleId, e.EntiteCibleId);
        Assert.Equal("Offre", e.EntiteCibleType);
        Assert.Equal("Offre créée", e.Details);
        Assert.Equal("127.0.0.1", e.AdresseIP);
        Assert.Equal("sess-001", e.SessionId);
    }

    [Fact]
    public void NouvelleInstance_DateHeure_EstRécenteUTC()
    {
        var avant = DateTime.UtcNow.AddSeconds(-1);
        var e = new EntreeJournal(Guid.NewGuid(), TypeOperation.CreationCompte);
        var après = DateTime.UtcNow.AddSeconds(1);

        Assert.InRange(e.DateHeure, avant, après);
    }
}
