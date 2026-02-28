using CrisisConnect.Domain.Interfaces.Services;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>
/// Adaptateur corpus interne — pas de traduction automatique.
/// Source prioritaire : textes gérés nativement dans l'application.
/// §5 ex.15 : corpus interne = défaut (priorité sur traducteur automatique).
/// </summary>
public class AdaptateurCorpusInterne : IServiceTraduction
{
    // Corpus simplifié : en production, charger depuis DB ou fichiers ressources
    private static readonly Dictionary<(string source, string cible, string texte), string> _corpus = new();

    public Task<string> TraduireAsync(string texte, string langueSource, string langueCible, CancellationToken cancellationToken = default)
    {
        if (_corpus.TryGetValue((langueSource, langueCible, texte), out var traduction))
            return Task.FromResult(traduction);

        // Corpus interne ne connaît pas ce texte — retourner l'original
        return Task.FromResult(texte);
    }

    public Task<IReadOnlyList<string>> LanguesSupporteesAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<string> langues = ["fr", "en", "nl", "de"];
        return Task.FromResult(langues);
    }

    public bool IsTraductionAuto() => false;
}
