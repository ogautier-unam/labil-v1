namespace CrisisConnect.Application.DTOs;

public record RegisterRequest(
    string Email,
    string MotDePasse,
    string Role,
    string Prenom,
    string Nom,
    string? Telephone = null);

public record LoginRequest(string Email, string MotDePasse);

public record AuthResponse(
    Guid UserId,
    string Email,
    string Role,
    string AccessToken,
    string RefreshToken);

public record RefreshTokenRequest(string RefreshToken);
