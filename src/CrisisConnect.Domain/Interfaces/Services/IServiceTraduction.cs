namespace CrisisConnect.Domain.Interfaces.Services;

/// <summary>
/// Pattern Adapter (GoF) — abstraction du service de traduction.
/// Implémentations : corpus interne (défaut), DeepL, LibreTranslate.
/// §5 ex.15-16 : indépendance fournisseur obligatoire.
/// </summary>
public interface IServiceTraduction
{
    Task<string> TraduireAsync(string texte, string langueSource, string langueCible, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> LanguesSupporteesAsync(CancellationToken cancellationToken = default);
    bool IsTraductionAuto();
}
