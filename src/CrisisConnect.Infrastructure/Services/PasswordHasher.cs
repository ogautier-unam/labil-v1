using CrisisConnect.Domain.Interfaces.Services;

namespace CrisisConnect.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    public string Hacher(string motDePasse) => BCrypt.Net.BCrypt.HashPassword(motDePasse);
    public bool Verifier(string motDePasse, string hash) => BCrypt.Net.BCrypt.Verify(motDePasse, hash);
}
