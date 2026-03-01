using CrisisConnect.Application.DTOs;
using Mediator;

namespace CrisisConnect.Application.UseCases.Auth.RefreshToken;

public record RefreshTokenCommand(string Token) : IRequest<AuthResponse>;
