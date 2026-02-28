using System.Net.Http.Headers;
using System.Security.Claims;

namespace CrisisConnect.Web.Services;

/// <summary>
/// DelegatingHandler qui injecte automatiquement le Bearer token JWT
/// (stocké dans le claim "access_token" du cookie d'authentification) dans chaque
/// requête HTTP émise vers l'API.
/// </summary>
public class JwtCookieHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _contextAccessor;

    public JwtCookieHandler(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _contextAccessor.HttpContext?.User?.FindFirstValue("access_token");
        if (token is not null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return base.SendAsync(request, cancellationToken);
    }
}
