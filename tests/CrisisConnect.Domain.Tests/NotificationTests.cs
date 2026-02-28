using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Tests;

public class NotificationTests
{
    [Fact]
    public void Notification_NouvelleInstance_EstNonLue()
    {
        var notif = new Notification(Guid.NewGuid(), TypeNotification.IntentionDonAcceptee, "Contenu test");

        Assert.False(notif.EstLue);
        Assert.Null(notif.DateEnvoi);
    }

    [Fact]
    public void MarquerCommeLue_NotifNonLue_EstLueEstVrai()
    {
        var notif = new Notification(Guid.NewGuid(), TypeNotification.IntentionDonRefusee, "Don refusé");

        notif.MarquerCommeLue();

        Assert.True(notif.EstLue);
    }

    [Fact]
    public void MarquerEnvoyee_Appelée_DateEnvoiRenseignée()
    {
        var notif = new Notification(Guid.NewGuid(), TypeNotification.SuggestionAppariementDisponible, "Suggestion dispo");

        notif.MarquerEnvoyee();

        Assert.NotNull(notif.DateEnvoi);
        Assert.True(notif.DateEnvoi <= DateTime.UtcNow);
    }

    [Fact]
    public void Notification_AvecRefEntiteId_PropriétésCorrectement()
    {
        var destinataireId = Guid.NewGuid();
        var refId = Guid.NewGuid().ToString();

        var notif = new Notification(destinataireId, TypeNotification.PanierAnnuleOffreRemiseActive, "Offre remise", refId);

        Assert.Equal(destinataireId, notif.DestinataireId);
        Assert.Equal(TypeNotification.PanierAnnuleOffreRemiseActive, notif.Type);
        Assert.Equal(refId, notif.RefEntiteId);
    }
}
