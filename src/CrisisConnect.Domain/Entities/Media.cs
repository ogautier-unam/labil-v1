using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Entities;

/// <summary>
/// Média attaché à une Proposition (photo, vidéo, audio, document).
/// §5.1 ex.7 : photos, vidéos, enregistrements depuis smartphone.
/// </summary>
public class Media
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid PropositionId { get; private set; }
    public string Url { get; private set; } = string.Empty;
    public TypeMedia Type { get; private set; }
    public DateTime DateAjout { get; private set; } = DateTime.UtcNow;

    protected Media() { }

    public Media(Guid propositionId, string url, TypeMedia type)
    {
        PropositionId = propositionId;
        Url = url;
        Type = type;
    }
}
