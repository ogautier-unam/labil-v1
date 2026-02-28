namespace CrisisConnect.Domain.ValueObjects;

public record PlageTemporelle(DateTime Debut, DateTime Fin)
{
    public TimeSpan Duree => Fin - Debut;
    public bool Chevauche(PlageTemporelle autre) => Debut < autre.Fin && Fin > autre.Debut;
}
