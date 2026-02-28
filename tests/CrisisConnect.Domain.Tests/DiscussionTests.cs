using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Tests;

public class DiscussionTests
{
    // Discussion a un constructeur internal : on la crée via Transaction
    private static Discussion ObtenirDiscussion()
    {
        var tx = new Transaction(Guid.NewGuid(), Guid.NewGuid());
        return tx.Discussion;
    }

    [Fact]
    public void Discussion_NouvelleInstance_EstPubliqueEtSansMessages()
    {
        var discussion = ObtenirDiscussion();

        Assert.Equal(Visibilite.Publique, discussion.Visibilite);
        Assert.Empty(discussion.Messages);
    }

    [Fact]
    public void AjouterMessage_ContenuValide_MessageAjoutéEtRetourné()
    {
        var discussion = ObtenirDiscussion();
        var expediteurId = Guid.NewGuid();

        var message = discussion.AjouterMessage(expediteurId, "Bonjour !");

        Assert.Single(discussion.Messages);
        Assert.Equal(expediteurId, message.ExpediteurId);
        Assert.Equal("Bonjour !", message.Contenu);
        Assert.Equal("fr", message.Langue);
    }

    [Fact]
    public void AjouterMessage_PlusieursMessages_TousConservés()
    {
        var discussion = ObtenirDiscussion();
        var exp = Guid.NewGuid();

        discussion.AjouterMessage(exp, "Premier");
        discussion.AjouterMessage(exp, "Deuxième");
        discussion.AjouterMessage(exp, "Troisième");

        Assert.Equal(3, discussion.Messages.Count);
    }

    [Fact]
    public void BasculerVisibilite_VersPrive_VisibilitePrivee()
    {
        var discussion = ObtenirDiscussion();

        discussion.BasculerVisibilite(Visibilite.Privee);

        Assert.Equal(Visibilite.Privee, discussion.Visibilite);
    }

    [Fact]
    public void BasculerVisibilite_RetourPublique_VisibilitePublique()
    {
        var discussion = ObtenirDiscussion();
        discussion.BasculerVisibilite(Visibilite.Privee);

        discussion.BasculerVisibilite(Visibilite.Publique);

        Assert.Equal(Visibilite.Publique, discussion.Visibilite);
    }
}
