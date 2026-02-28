using CrisisConnect.Domain.ValueObjects;

namespace CrisisConnect.Domain.Entities;

public class Personne : Acteur
{
    public string Prenom { get; private set; } = string.Empty;
    public string Nom { get; private set; } = string.Empty;
    public string? Telephone { get; private set; }
    public Adresse? Adresse { get; private set; }

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
}
