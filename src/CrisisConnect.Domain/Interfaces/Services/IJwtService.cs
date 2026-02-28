using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Services;

public interface IJwtService
{
    string GenererAccessToken(Personne personne);
    string GenererRefreshToken();
}
