using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;

namespace CrisisConnect.Domain.Entities;

public class Panier
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProprietaireId { get; private set; }
    public StatutPanier Statut { get; private set; } = StatutPanier.Ouvert;
    public DateTime DateCreation { get; private set; } = DateTime.UtcNow;
    public DateTime? DateConfirmation { get; private set; }

    private readonly List<Offre> _offres = [];
    public IReadOnlyCollection<Offre> Offres => _offres.AsReadOnly();

    protected Panier() { }

    public Panier(Guid proprietaireId)
    {
        ProprietaireId = proprietaireId;
    }

    public void AjouterOffre(Offre offre)
    {
        if (Statut != StatutPanier.Ouvert)
            throw new DomainException("Le panier n'est pas ouvert.");
        if (_offres.Any(o => o.Id == offre.Id))
            throw new DomainException("L'offre est déjà dans le panier.");
        _offres.Add(offre);
    }

    public void Confirmer()
    {
        if (Statut != StatutPanier.Ouvert)
            throw new DomainException("Seul un panier ouvert peut être confirmé.");
        Statut = StatutPanier.Confirme;
        DateConfirmation = DateTime.UtcNow;
    }

    public void Annuler()
    {
        if (Statut == StatutPanier.Annule)
            throw new DomainException("Le panier est déjà annulé.");
        Statut = StatutPanier.Annule;
    }
}
