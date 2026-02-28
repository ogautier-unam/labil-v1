using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Entities;

public class Discussion
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid TransactionId { get; private set; }
    public Visibilite Visibilite { get; private set; } = Visibilite.Publique;
    public DateTime DateCreation { get; private set; } = DateTime.UtcNow;

    private readonly List<Message> _messages = [];
    public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

    // Constructeur interne : utilisé par Transaction (même assembly) et EF Core (réflexion)
    internal Discussion() { }

    public void BasculerVisibilite(Visibilite nouvelleVisibilite)
    {
        Visibilite = nouvelleVisibilite;
    }

    public Message AjouterMessage(Guid expediteurId, string contenu, string langue = "fr")
    {
        var message = new Message(Id, expediteurId, contenu, langue);
        _messages.Add(message);
        return message;
    }
}
