using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Auth.Register;

public record RegisterActeurCommand(
    string Email,
    string MotDePasse,
    string Role,
    string Prenom,
    string Nom,
    string? Telephone = null) : IRequest<AuthResponse>;
