namespace CrisisConnect.Domain.Entities;

public abstract class Acteur
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public string Email { get; protected set; } = string.Empty;
    public string MotDePasseHash { get; protected set; } = string.Empty;
    public string Role { get; protected set; } = string.Empty;
    public bool EstActif { get; protected set; } = true;
    public DateTime CreeLe { get; protected set; } = DateTime.UtcNow;
    public DateTime? ModifieLe { get; protected set; }
}
