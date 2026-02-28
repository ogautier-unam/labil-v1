using CrisisConnect.Application.DTOs;
using MediatR;

namespace CrisisConnect.Application.UseCases.Auth.RefreshToken;

public record RefreshTokenCommand(string Token) : IRequest<AuthResponse>;
