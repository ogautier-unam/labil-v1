namespace CrisisConnect.Domain.Entities;

/// <summary>
/// Suggestion automatique d'appariement offre ↔ demande.
/// §5.2 ex.4 : générée "lorsque les connaissances le permettent",
/// basée sur géolocalisation + nature de la proposition.
/// </summary>
public class SuggestionAppariement
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid OffreId { get; private set; }
    public Guid DemandeId { get; private set; }
    public DateTime DateGeneration { get; private set; } = DateTime.UtcNow;
    public double ScoreCorrespondance { get; private set; }
    public string Raisonnement { get; private set; } = string.Empty;
    public bool EstAcknowledged { get; private set; }

    protected SuggestionAppariement() { }

    public SuggestionAppariement(Guid offreId, Guid demandeId, double score, string raisonnement)
    {
        OffreId = offreId;
        DemandeId = demandeId;
        ScoreCorrespondance = score;
        Raisonnement = raisonnement;
    }

    public void Acknowledger() => EstAcknowledged = true;
}
