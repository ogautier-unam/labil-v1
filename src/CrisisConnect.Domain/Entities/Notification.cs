using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid DestinataireId { get; private set; }
    public TypeNotification Type { get; private set; }
    public string Contenu { get; private set; } = string.Empty;
    public string? RefEntiteId { get; private set; }
    public bool EstLue { get; private set; }
    public DateTime DateCreation { get; private set; } = DateTime.UtcNow;
    public DateTime? DateEnvoi { get; private set; }

    protected Notification() { }

    public Notification(Guid destinataireId, TypeNotification type, string contenu, string? refEntiteId = null)
    {
        DestinataireId = destinataireId;
        Type = type;
        Contenu = contenu;
        RefEntiteId = refEntiteId;
    }

    public void MarquerCommeLue()
    {
        EstLue = true;
    }

    public void MarquerEnvoyee()
    {
        DateEnvoi = DateTime.UtcNow;
    }
}
