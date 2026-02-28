namespace CrisisConnect.Domain.Enums;

/// <summary>
/// Niveau de fiabilité d'une méthode d'identification.
/// §5 ex.14 : visible sur le profil public, détermine le badge VERT/ORANGE/ROUGE.
/// </summary>
public enum NiveauFiabilite
{
    TresHaute,     // eID, virement bancaire
    Haute,         // SMS, téléphone
    Moyenne,       // facture, photo
    Faible,        // parrainage
    ExplicitementFaible  // délégation — dernier recours (§5 ex.7)
}
