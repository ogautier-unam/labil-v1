namespace CrisisConnect.Domain.Interfaces.Services;

public interface IPasswordHasher
{
    string Hacher(string motDePasse);
    bool Verifier(string motDePasse, string hash);
}
