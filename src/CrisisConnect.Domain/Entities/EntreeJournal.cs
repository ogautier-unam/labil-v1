using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Entities;

public class EntreeJournal
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime DateHeure { get; private set; } = DateTime.UtcNow;
    public TypeOperation TypeOperation { get; private set; }
    public Guid ActeurId { get; private set; }
    public Guid? EntiteCibleId { get; private set; }
    public string? EntiteCibleType { get; private set; }
    public string? Details { get; private set; }
    public string? AdresseIP { get; private set; }
    public string? SessionId { get; private set; }

    protected EntreeJournal() { }

    public EntreeJournal(Guid acteurId, TypeOperation typeOperation,
        Guid? entiteCibleId = null, string? entiteCibleType = null,
        string? details = null, string? adresseIP = null, string? sessionId = null)
    {
        ActeurId = acteurId;
        TypeOperation = typeOperation;
        EntiteCibleId = entiteCibleId;
        EntiteCibleType = entiteCibleType;
        Details = details;
        AdresseIP = adresseIP;
        SessionId = sessionId;
    }
}
