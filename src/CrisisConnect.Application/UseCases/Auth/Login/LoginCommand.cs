using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Auth.Login;

public record LoginCommand(string Email, string MotDePasse) : IRequest<AuthResponse>;
