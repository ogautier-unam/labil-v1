using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using Mediator;
using DomainRefreshToken = CrisisConnect.Domain.Entities.RefreshToken;

namespace CrisisConnect.Application.UseCases.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IPersonneRepository _personneRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IPersonneRepository personneRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService,
        IPasswordHasher passwordHasher)
    {
        _personneRepository = personneRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async ValueTask<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var personne = await _personneRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (personne is null || !_passwordHasher.Verifier(request.MotDePasse, personne.MotDePasseHash))
            throw new DomainException("Email ou mot de passe incorrect.");

        await _refreshTokenRepository.RevoquerTousAsync(personne.Id, cancellationToken);

        var accessToken = _jwtService.GenererAccessToken(personne);
        var refreshTokenStr = _jwtService.GenererRefreshToken();
        var refreshToken = new DomainRefreshToken(refreshTokenStr, personne.Id, DateTime.UtcNow.AddDays(7));
        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        return new AuthResponse(personne.Id, personne.Email, personne.Role, accessToken, refreshTokenStr);
    }
}
