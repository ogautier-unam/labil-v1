using System.Security.Claims;
using CrisisConnect.Application.Common.Interfaces;

namespace CrisisConnect.API.Services;

public class CurrentUserService : ICurrentUserService
{
    public Guid? UserId { get; }
    public string? Role { get; }
    public bool IsAuthenticated { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;
        IsAuthenticated = user?.Identity?.IsAuthenticated ?? false;

        var subClaim = user?.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user?.FindFirstValue("sub");
        if (Guid.TryParse(subClaim, out var id))
            UserId = id;

        Role = user?.FindFirstValue(ClaimTypes.Role);
    }
}
