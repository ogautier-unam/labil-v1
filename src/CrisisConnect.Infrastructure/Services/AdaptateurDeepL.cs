using CrisisConnect.Domain.Interfaces.Services;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>
/// Adaptateur DeepL — traduction automatique via API DeepL Pro.
/// §5 ex.15-16 : traduction auto avec mention obligatoire ; indépendance fournisseur.
/// </summary>
public class AdaptateurDeepL : IServiceTraduction
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public AdaptateurDeepL(IHttpClientFactory httpClientFactory, string apiKey)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(AdaptateurDeepL));
        _apiKey = apiKey;
    }

    public async Task<string> TraduireAsync(string texte, string langueSource, string langueCible, CancellationToken cancellationToken = default)
    {
        // Implémentation simplifiée — à compléter avec l'API DeepL réelle
        var content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("auth_key", _apiKey),
            new KeyValuePair<string, string>("text", texte),
            new KeyValuePair<string, string>("source_lang", langueSource.ToUpperInvariant()),
            new KeyValuePair<string, string>("target_lang", langueCible.ToUpperInvariant()),
        ]);

        var response = await _httpClient.PostAsync("https://api-free.deepl.com/v2/translate", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        // Parse simplifiée — en production utiliser un DTO JSON
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        return body; // TODO: extraire le champ "text" du JSON
    }

    public Task<IReadOnlyList<string>> LanguesSupporteesAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<string> langues = ["fr", "en", "de", "nl", "es", "it", "pt", "pl", "ru", "ja", "zh"];
        return Task.FromResult(langues);
    }

    public bool IsTraductionAuto() => true;
}
