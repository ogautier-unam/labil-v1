using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using MediatR;
using DomainRefreshToken = CrisisConnect.Domain.Entities.RefreshToken;

namespace CrisisConnect.Application.UseCases.Auth.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPersonneRepository _personneRepository;
    private readonly IJwtService _jwtService;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IPersonneRepository personneRepository,
        IJwtService jwtService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _personneRepository = personneRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.Token, cancellationToken);
        if (refreshToken is null || !refreshToken.EstValide)
            throw new DomainException("Refresh token invalide ou expiré.");

        var personne = await _personneRepository.GetByIdAsync(refreshToken.PersonneId, cancellationToken);
        if (personne is null)
            throw new NotFoundException(nameof(Personne), refreshToken.PersonneId);

        refreshToken.Revoquer();
        await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

        var newAccessToken = _jwtService.GenererAccessToken(personne);
        var newRefreshTokenStr = _jwtService.GenererRefreshToken();
        var newRefreshToken = new DomainRefreshToken(newRefreshTokenStr, personne.Id, DateTime.UtcNow.AddDays(7));
        await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

        return new AuthResponse(personne.Id, personne.Email, personne.Role, newAccessToken, newRefreshTokenStr);
    }
}
