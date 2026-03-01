using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Suggestions.GenererSuggestions;

public class GenererSuggestionsCommandHandler
    : IRequestHandler<GenererSuggestionsCommand, IReadOnlyList<SuggestionAppariementDto>>
{
    private readonly IDemandeRepository _demandeRepository;
    private readonly IOffreRepository _offreRepository;
    private readonly ISuggestionAppariementRepository _suggestionRepository;
    private readonly AppMapper _mapper;

    private const double SeuilMinimum = 0.10;

    public GenererSuggestionsCommandHandler(
        IDemandeRepository demandeRepository,
        IOffreRepository offreRepository,
        ISuggestionAppariementRepository suggestionRepository,
        AppMapper mapper)
    {
        _demandeRepository = demandeRepository;
        _offreRepository = offreRepository;
        _suggestionRepository = suggestionRepository;
        _mapper = mapper;
    }

    public async ValueTask<IReadOnlyList<SuggestionAppariementDto>> Handle(
        GenererSuggestionsCommand request,
        CancellationToken cancellationToken)
    {
        var demande = await _demandeRepository.GetByIdAsync(request.DemandeId, cancellationToken)
            ?? throw new NotFoundException(nameof(Demande), request.DemandeId);

        // Seules les demandes actives peuvent recevoir des suggestions
        if (demande.Statut != StatutProposition.Active)
            return [];

        var offres = await _offreRepository.GetAllAsync(cancellationToken);
        var offresActives = offres.Where(o => o.Statut == StatutProposition.Active).ToList();

        // Éviter les doublons : ne pas régénérer une suggestion déjà existante
        var existantes = await _suggestionRepository.GetByDemandeAsync(request.DemandeId, cancellationToken);
        var offreIdsDejaTraites = existantes.Select(s => s.OffreId).ToHashSet();

        var nouvelles = new List<SuggestionAppariement>();

        foreach (var offre in offresActives)
        {
            if (offreIdsDejaTraites.Contains(offre.Id))
                continue;

            var (score, raisonnement) = CalculerScore(offre, demande);
            if (score < SeuilMinimum)
                continue;

            var suggestion = new SuggestionAppariement(offre.Id, demande.Id, score, raisonnement);
            await _suggestionRepository.AddAsync(suggestion, cancellationToken);
            nouvelles.Add(suggestion);
        }

        return _mapper.ToDto(nouvelles);
    }

    /// <summary>
    /// Calcule un score de correspondance [0,1] entre une offre et une demande.
    /// Critères : similarité textuelle (Jaccard), niveau d'urgence, livraison.
    /// </summary>
    private static (double score, string raisonnement) CalculerScore(Offre offre, Demande demande)
    {
        var motsDemande = TokeniserTexte(demande.Titre + " " + demande.Description);
        var motsOffre   = TokeniserTexte(offre.Titre   + " " + offre.Description);

        var intersection = motsDemande.Intersect(motsOffre).Count();
        var union        = motsDemande.Union(motsOffre).Count();
        double scoreSimilarite = union == 0 ? 0.0 : (double)intersection / union;

        // Bonus urgence : les demandes critiques méritent plus de visibilité
        double bonusUrgence = demande.Urgence switch
        {
            NiveauUrgence.Critique => 0.20,
            NiveauUrgence.Eleve    => 0.10,
            _                      => 0.0
        };

        // Bonus livraison : valorise les offres avec livraison incluse
        double bonusLivraison = offre.LivraisonIncluse ? 0.05 : 0.0;

        var score = Math.Round(Math.Min(1.0, scoreSimilarite + bonusUrgence + bonusLivraison), 2);

        var raisons = new List<string>();
        if (intersection > 0)    raisons.Add($"{intersection} mot(s) commun(s)");
        if (bonusUrgence > 0)    raisons.Add($"urgence {demande.Urgence}");
        if (bonusLivraison > 0)  raisons.Add("livraison incluse");
        if (raisons.Count == 0)  raisons.Add("correspondance de base");

        return (score, string.Join(", ", raisons));
    }

    private static HashSet<string> TokeniserTexte(string texte)
        => texte.ToLowerInvariant()
                .Split([' ', ',', '.', ';', ':', '!', '?', '\n', '\r'],
                       StringSplitOptions.RemoveEmptyEntries)
                .Where(m => m.Length >= 3)
                .ToHashSet();
}
