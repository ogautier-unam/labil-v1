using CrisisConnect.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>
/// Service de notification — envoi externe (email, push, SMS).
/// L'implémentation courante logue la notification ; à remplacer
/// par un vrai fournisseur (SendGrid, Firebase, etc.) en production.
/// </summary>
public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger;
    }

    public Task EnvoyerAsync(Guid destinataireId, string sujet, string contenu, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Notification → destinataire={DestinataireId} | sujet={Sujet} | contenu={Contenu}",
            destinataireId, sujet, contenu);
        return Task.CompletedTask;
    }
}
