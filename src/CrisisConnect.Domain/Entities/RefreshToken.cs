namespace CrisisConnect.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Token { get; private set; } = string.Empty;
    public Guid PersonneId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool EstRevoque { get; private set; }
    public DateTime CreeLe { get; private set; } = DateTime.UtcNow;

    protected RefreshToken() { }

    public RefreshToken(string token, Guid personneId, DateTime expiresAt)
    {
        Token = token;
        PersonneId = personneId;
        ExpiresAt = expiresAt;
    }

    public bool EstValide => !EstRevoque && ExpiresAt > DateTime.UtcNow;

    public void Revoquer()
    {
        EstRevoque = true;
    }
}
