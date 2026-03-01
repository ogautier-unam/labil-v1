using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.ValueObjects;

namespace CrisisConnect.Domain.Entities;

public class Personne : Acteur
{
    public string Prenom { get; private set; } = string.Empty;
    public string Nom { get; private set; } = string.Empty;
    public string? Telephone { get; private set; }
    public Adresse? Adresse { get; private set; }
    public string? UrlPhoto { get; private set; }
    public string? LanguePreferee { get; private set; }
    public string? MoyensContact { get; private set; }

    private readonly List<MethodeIdentification> _methodesIdentification = [];
    public IReadOnlyList<MethodeIdentification> MethodesIdentification => _methodesIdentification.AsReadOnly();

    protected Personne() { }

    public Personne(string email, string motDePasseHash, string role, string prenom, string nom)
    {
        Email = email;
        MotDePasseHash = motDePasseHash;
        Role = role;
        Prenom = prenom;
        Nom = nom;
    }

    public string NomComplet => $"{Prenom} {Nom}";

    /// <summary>
    /// Badge basé sur la meilleure méthode d'identification vérifiée (§5 ex.14).
    /// TresHaute/Haute → Vert · Moyenne → Orange · Faible/ExplicitementFaible → Rouge.
    /// </summary>
    public override NiveauBadge GetNiveauBadge()
    {
        var best = _methodesIdentification
            .Where(m => m.EstVerifiee)
            .OrderBy(m => m.NiveauFiabilite)  // enum order : TresHaute=0 est le meilleur
            .FirstOrDefault();

        return best?.NiveauFiabilite switch
        {
            NiveauFiabilite.TresHaute or NiveauFiabilite.Haute => NiveauBadge.Vert,
            NiveauFiabilite.Moyenne => NiveauBadge.Orange,
            _ => NiveauBadge.Rouge
        };
    }
}
