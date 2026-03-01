using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.ValueObjects;

namespace CrisisConnect.Domain.Entities;

public abstract class Proposition
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public string Titre { get; protected set; } = string.Empty;
    public string Description { get; protected set; } = string.Empty;
    public StatutProposition Statut { get; protected set; } = StatutProposition.Active;
    public Localisation? Localisation { get; protected set; }
    public Guid CreePar { get; protected set; }
    public Guid? CategorieId { get; protected set; }
    public DateTime CreeLe { get; protected set; } = DateTime.UtcNow;
    public DateTime? ModifieLe { get; protected set; }
    public DateTime? DateRelance { get; protected set; }
    public DateTime? DateArchivage { get; protected set; }
    public DateTime? DateCloture { get; protected set; }

    private readonly List<Media> _medias = [];
    public IReadOnlyCollection<Media> Medias => _medias.AsReadOnly();

    public abstract void Clore();

    public void AjouterMedia(string url, TypeMedia type)
    {
        _medias.Add(new Media(Id, url, type));
    }

    public void Archiver()
    {
        if (Statut == StatutProposition.Cloturee)
            throw new DomainException("Une proposition clôturée ne peut pas être archivée.");
        Statut = StatutProposition.Archivee;
        DateArchivage = DateTime.UtcNow;
        ModifieLe = DateTime.UtcNow;
    }

    public void MarquerEnAttenteRelance()
    {
        if (Statut != StatutProposition.Active)
            throw new DomainException("Seule une proposition active peut être mise en attente de relance.");
        Statut = StatutProposition.EnAttenteRelance;
        DateRelance = DateTime.UtcNow;
        ModifieLe = DateTime.UtcNow;
    }

    public void Reconfirmer()
    {
        if (Statut != StatutProposition.EnAttenteRelance)
            throw new DomainException("La proposition n'est pas en attente de relance.");
        Statut = StatutProposition.Active;
        ModifieLe = DateTime.UtcNow;
    }

    public void MarquerEnTransaction()
    {
        if (Statut != StatutProposition.Active)
            throw new DomainException("Seule une proposition active peut être engagée dans une transaction.");
        Statut = StatutProposition.EnTransaction;
        ModifieLe = DateTime.UtcNow;
    }

    public void LibererDeTransaction()
    {
        if (Statut != StatutProposition.EnTransaction)
            throw new DomainException("La proposition n'est pas en transaction.");
        Statut = StatutProposition.Active;
        ModifieLe = DateTime.UtcNow;
    }

    /// <summary>Recycle une proposition archivée vers Active sans limite de temps (§5.1 ex.1).</summary>
    public void Recycler()
    {
        if (Statut != StatutProposition.Archivee)
            throw new DomainException("Seule une proposition archivée peut être recyclée.");
        Statut = StatutProposition.Active;
        DateArchivage = null;
        ModifieLe = DateTime.UtcNow;
    }
}
