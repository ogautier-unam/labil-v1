using CrisisConnect.Domain.Enums;

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

    /// <summary>Badge d'authenticité basé sur le meilleur niveau d'identification vérifié (§5 ex.14).</summary>
    public abstract NiveauBadge GetNiveauBadge();
}
