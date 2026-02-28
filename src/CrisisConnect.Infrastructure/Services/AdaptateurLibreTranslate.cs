using System.Text;
using System.Text.Json;
using CrisisConnect.Domain.Interfaces.Services;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>
/// Adaptateur LibreTranslate — traduction automatique open source, hébergeable localement.
/// §5 ex.15-16 : open source, cohérent avec la licence OS du projet.
/// </summary>
public class AdaptateurLibreTranslate : IServiceTraduction
{
    private readonly HttpClient _httpClient;
    private readonly string _urlServeur;

    public AdaptateurLibreTranslate(IHttpClientFactory httpClientFactory, string urlServeur)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(AdaptateurLibreTranslate));
        _urlServeur = urlServeur.TrimEnd('/');
    }

    public async Task<string> TraduireAsync(string texte, string langueSource, string langueCible, CancellationToken cancellationToken = default)
    {
        var payload = new { q = texte, source = langueSource, target = langueCible, format = "text" };
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_urlServeur}/translate", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.GetProperty("translatedText").GetString() ?? texte;
    }

    public async Task<IReadOnlyList<string>> LanguesSupporteesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{_urlServeur}/languages", cancellationToken);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(body);
        var langues = doc.RootElement.EnumerateArray()
            .Select(e => e.GetProperty("code").GetString() ?? "")
            .Where(c => !string.IsNullOrEmpty(c))
            .ToList();
        return langues;
    }

    public bool IsTraductionAuto() => true;
}
