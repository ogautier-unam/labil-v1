using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Entities;

/// <summary>
/// Méthode d'identification d'une Personne — Pattern TPH.
/// §5 ex.7 : utilisée UNIQUEMENT pour (1) création de compte ou (2) rétablissement de confiance.
/// §5 ex.14 : badge VERT/ORANGE/ROUGE déterminé par le NiveauFiabilite.
/// </summary>
public abstract class MethodeIdentification
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public Guid PersonneId { get; protected set; }
    public DateTime DateVerification { get; protected set; } = DateTime.UtcNow;
    public bool EstVerifiee { get; protected set; }
    public NiveauFiabilite NiveauFiabilite { get; protected set; }

    protected MethodeIdentification() { }

    protected MethodeIdentification(Guid personneId, NiveauFiabilite niveauFiabilite)
    {
        PersonneId = personneId;
        NiveauFiabilite = niveauFiabilite;
    }

    public void MarquerVerifiee()
    {
        EstVerifiee = true;
        DateVerification = DateTime.UtcNow;
    }
}

/// <summary>§5 ex.8 : minimum obligatoire — toujours présent.</summary>
public class LoginPassword : MethodeIdentification
{
    public string Login { get; private set; } = string.Empty;
    public string MotDePasseHash { get; private set; } = string.Empty;
    public DateTime? DerniereConnexion { get; private set; }

    protected LoginPassword() { }

    public LoginPassword(Guid personneId, string login, string motDePasseHash)
        : base(personneId, NiveauFiabilite.Faible)
    {
        Login = login;
        MotDePasseHash = motDePasseHash;
        EstVerifiee = true;
    }

    public void EnregistrerConnexion() => DerniereConnexion = DateTime.UtcNow;
}

/// <summary>§5 ex.7 & ex.14 — NiveauFiabilite : TRES_HAUTE.</summary>
public class CarteIdentiteElectronique : MethodeIdentification
{
    public string NumeroCarte { get; private set; } = string.Empty;
    public string PaysEmetteur { get; private set; } = string.Empty;

    protected CarteIdentiteElectronique() { }

    public CarteIdentiteElectronique(Guid personneId, string numeroCarte, string paysEmetteur)
        : base(personneId, NiveauFiabilite.TresHaute)
    {
        NumeroCarte = numeroCarte;
        PaysEmetteur = paysEmetteur;
    }
}

/// <summary>§5 ex.7 : challenge par SMS ou messagerie. NiveauFiabilite : HAUTE.</summary>
public class VerificationSMS : MethodeIdentification
{
    public string NumeroTelephone { get; private set; } = string.Empty;

    protected VerificationSMS() { }

    public VerificationSMS(Guid personneId, string numeroTelephone)
        : base(personneId, NiveauFiabilite.Haute)
    {
        NumeroTelephone = numeroTelephone;
    }
}

/// <summary>§5 ex.7 : virement de 1 centime. NiveauFiabilite : HAUTE.</summary>
public class VerificationBancaire : MethodeIdentification
{
    public string ReferenceVirement { get; private set; } = string.Empty;
    public int MontantVireCentimes { get; private set; }

    protected VerificationBancaire() { }

    public VerificationBancaire(Guid personneId, string referenceVirement, int montantVireCentimes)
        : base(personneId, NiveauFiabilite.Haute)
    {
        ReferenceVirement = referenceVirement;
        MontantVireCentimes = montantVireCentimes;
    }
}

/// <summary>§5 ex.7 : facture d'électricité récente. NiveauFiabilite : MOYENNE.</summary>
public class VerificationFacture : MethodeIdentification
{
    public TypeFacture TypeFacture { get; private set; }
    public DateTime DateFacture { get; private set; }

    protected VerificationFacture() { }

    public VerificationFacture(Guid personneId, TypeFacture typeFacture, DateTime dateFacture)
        : base(personneId, NiveauFiabilite.Moyenne)
    {
        TypeFacture = typeFacture;
        DateFacture = dateFacture;
    }
}

/// <summary>§5 ex.14 : photo carte identité ± mise en contexte. NiveauFiabilite : MOYENNE.</summary>
public class VerificationPhoto : MethodeIdentification
{
    public string UrlPhoto { get; private set; } = string.Empty;
    public bool IncluPersonne { get; private set; }
    public ModeVerification ModeVerification { get; private set; }

    protected VerificationPhoto() { }

    public VerificationPhoto(Guid personneId, string urlPhoto, bool incluPersonne, ModeVerification mode)
        : base(personneId, NiveauFiabilite.Moyenne)
    {
        UrlPhoto = urlPhoto;
        IncluPersonne = incluPersonne;
        ModeVerification = mode;
    }
}

/// <summary>§5 ex.14 : parrainage par n utilisateurs identifiés. NiveauFiabilite : variable.</summary>
public class Parrainage : MethodeIdentification
{
    public int NombreParrainsRequis { get; private set; }

    private readonly List<Guid> _parrainsIds = [];
    public IReadOnlyCollection<Guid> ParrainsIds => _parrainsIds.AsReadOnly();
    public int NombreParrainsActuels => _parrainsIds.Count;

    protected Parrainage() { }

    public Parrainage(Guid personneId, int nombreParrainsRequis)
        : base(personneId, NiveauFiabilite.Faible)
    {
        NombreParrainsRequis = nombreParrainsRequis;
    }

    public void AjouterParrain(Guid parrain)
    {
        if (!_parrainsIds.Contains(parrain))
            _parrainsIds.Add(parrain);
        if (_parrainsIds.Count >= NombreParrainsRequis)
            MarquerVerifiee();
    }
}

/// <summary>
/// §5 ex.7 : personne déjà authentifiée atteste de l'identité d'une tierce.
/// NiveauFiabilite : EXPLICITEMENT_FAIBLE — dernier recours (sinistré ayant tout perdu).
/// </summary>
public class Delegation : MethodeIdentification
{
    public Guid GarantId { get; private set; }

    protected Delegation() { }

    public Delegation(Guid personneId, Guid garantId)
        : base(personneId, NiveauFiabilite.ExplicitementFaible)
    {
        GarantId = garantId;
        EstVerifiee = true; // Vérification immédiate par délégation
    }
}
