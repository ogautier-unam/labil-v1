namespace CrisisConnect.Domain.ValueObjects;

public record Adresse(
    string Rue,
    string Ville,
    string CodePostal,
    string Pays = "France");
