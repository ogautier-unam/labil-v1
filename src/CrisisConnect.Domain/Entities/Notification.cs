namespace CrisisConnect.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid DestinataireId { get; private set; }
    public string Sujet { get; private set; } = string.Empty;
    public string Contenu { get; private set; } = string.Empty;
    public bool EstLue { get; private set; }
    public DateTime CreeLe { get; private set; } = DateTime.UtcNow;

    protected Notification() { }

    public Notification(Guid destinataireId, string sujet, string contenu)
    {
        DestinataireId = destinataireId;
        Sujet = sujet;
        Contenu = contenu;
    }

    public void MarquerCommeLue()
    {
        EstLue = true;
    }
}
