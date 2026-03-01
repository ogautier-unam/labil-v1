using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Auth.Login;

public record LoginCommand(string Email, string MotDePasse) : IRequest<AuthResponse>;
