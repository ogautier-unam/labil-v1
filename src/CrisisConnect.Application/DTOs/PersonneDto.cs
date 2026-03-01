using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Application.DTOs;

public record PersonneDto(
    Guid Id,
    string Email,
    string Role,
    string Prenom,
    string Nom,
    string NomComplet,
    string? Telephone,
    string? UrlPhoto,
    string? LanguePreferee,
    string? MoyensContact,
    string? AdresseRue,
    string? AdresseVille,
    string? AdresseCodePostal,
    string? AdressePays,
    NiveauBadge NiveauBadge);
