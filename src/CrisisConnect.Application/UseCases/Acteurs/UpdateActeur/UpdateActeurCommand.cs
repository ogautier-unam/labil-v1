using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Acteurs.UpdateActeur;

public record UpdateActeurCommand(
    Guid Id,
    string Prenom,
    string Nom,
    string? Telephone = null,
    string? UrlPhoto = null,
    string? LanguePreferee = null,
    string? MoyensContact = null,
    string? AdresseRue = null,
    string? AdresseVille = null,
    string? AdresseCodePostal = null,
    string? AdressePays = null) : IRequest<PersonneDto>;
