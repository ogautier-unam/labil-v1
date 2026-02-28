using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Tests;

public class MessageTests
{
    [Fact]
    public void NouvelleInstance_ChampsDéfaut_Setés()
    {
        var discussionId = Guid.NewGuid();
        var expediteurId = Guid.NewGuid();

        var msg = new Message(discussionId, expediteurId, "Bonjour !");

        Assert.Equal(discussionId, msg.DiscussionId);
        Assert.Equal(expediteurId, msg.ExpediteurId);
        Assert.Equal("Bonjour !", msg.Contenu);
        Assert.Equal("fr", msg.Langue);
        Assert.False(msg.IssueTraductionAuto);
        Assert.Null(msg.TexteOriginal);
    }

    [Fact]
    public void NouvelleInstance_AvecTraduction_ChampsSetés()
    {
        var msg = new Message(Guid.NewGuid(), Guid.NewGuid(),
            "Hello!", langue: "en",
            issueTraductionAuto: true,
            texteOriginal: "Bonjour !");

        Assert.Equal("en", msg.Langue);
        Assert.True(msg.IssueTraductionAuto);
        Assert.Equal("Bonjour !", msg.TexteOriginal);
    }

    [Fact]
    public void NouvelleInstance_DateEnvoi_EstRécenteUTC()
    {
        var avant = DateTime.UtcNow.AddSeconds(-1);
        var msg = new Message(Guid.NewGuid(), Guid.NewGuid(), "Test");
        var après = DateTime.UtcNow.AddSeconds(1);

        Assert.InRange(msg.DateEnvoi, avant, après);
    }
}
