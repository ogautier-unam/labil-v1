namespace CrisisConnect.Domain.Entities;

/// <summary>
/// Configuration globale d'une instance de crise.
/// §5 ex.2 : configurée par l'AC selon la catastrophe en cours.
/// §4 hyp.1 : état référent unique par déploiement.
/// </summary>
public class ConfigCatastrophe
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nom { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ZoneGeographique { get; private set; } = string.Empty;
    public string EtatReferent { get; private set; } = string.Empty;
    public int DelaiArchivageJours { get; private set; } = 30;
    public int DelaiRappelAvantArchivage { get; private set; } = 7;
    public bool EstActive { get; private set; } = true;
    public DateTime DateDebut { get; private set; } = DateTime.UtcNow;

    // Stocké comme JSON ou chaîne délimitée — langues actives (codes ISO 639)
    public string LanguesActives { get; private set; } = "fr";
    // Modes d'identification activés (noms des classes MethodeIdentification)
    public string ModesIdentificationActifs { get; private set; } = "LoginPassword";

    protected ConfigCatastrophe() { }

    public ConfigCatastrophe(string nom, string description, string zoneGeographique, string etatReferent,
        int delaiArchivageJours = 30, int delaiRappelAvantArchivage = 7)
    {
        Nom = nom;
        Description = description;
        ZoneGeographique = zoneGeographique;
        EtatReferent = etatReferent;
        DelaiArchivageJours = delaiArchivageJours;
        DelaiRappelAvantArchivage = delaiRappelAvantArchivage;
    }

    public void Activer() => EstActive = true;
    public void Desactiver() => EstActive = false;

    public void MettreAJourParametres(int delaiArchivage, int delaiRappel)
    {
        DelaiArchivageJours = delaiArchivage;
        DelaiRappelAvantArchivage = delaiRappel;
    }
}
