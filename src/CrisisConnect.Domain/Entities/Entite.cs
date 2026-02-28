namespace CrisisConnect.Domain.Entities;

public class Entite : Acteur
{
    public string Nom { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string MoyensContact { get; private set; } = string.Empty;
    public string? UrlPagePresentation { get; private set; }
    public string? CommentFaireDon { get; private set; }
    public string? TypesContributions { get; private set; }
    public bool EstActive { get; private set; } = true;
    public Guid ResponsableId { get; private set; }

    protected Entite() { }

    public Entite(string email, string motDePasseHash, string nom, string description, string moyensContact, Guid responsableId)
    {
        Email = email;
        MotDePasseHash = motDePasseHash;
        Role = "Entite";
        Nom = nom;
        Description = description;
        MoyensContact = moyensContact;
        ResponsableId = responsableId;
    }

    public void Desactiver()
    {
        EstActive = false;
        ModifieLe = DateTime.UtcNow;
    }

    public void Reactiver()
    {
        EstActive = true;
        ModifieLe = DateTime.UtcNow;
    }
}
