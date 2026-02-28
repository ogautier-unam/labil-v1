using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using MediatR;
using DomainRefreshToken = CrisisConnect.Domain.Entities.RefreshToken;

namespace CrisisConnect.Application.UseCases.Auth.Register;

public class RegisterActeurCommandHandler : IRequestHandler<RegisterActeurCommand, AuthResponse>
{
    private readonly IPersonneRepository _personneRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterActeurCommandHandler(
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

    public async Task<AuthResponse> Handle(RegisterActeurCommand request, CancellationToken cancellationToken)
    {
        var existant = await _personneRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existant is not null)
            throw new DomainException($"Un acteur avec l'email '{request.Email}' existe déjà.");

        var hash = _passwordHasher.Hacher(request.MotDePasse);
        var personne = new Personne(request.Email, hash, request.Role, request.Prenom, request.Nom);

        await _personneRepository.AddAsync(personne, cancellationToken);

        var accessToken = _jwtService.GenererAccessToken(personne);
        var refreshTokenStr = _jwtService.GenererRefreshToken();
        var refreshToken = new DomainRefreshToken(refreshTokenStr, personne.Id, DateTime.UtcNow.AddDays(7));
        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        return new AuthResponse(personne.Id, personne.Email, personne.Role, accessToken, refreshTokenStr);
    }
}
