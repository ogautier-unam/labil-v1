namespace CrisisConnect.Domain.Interfaces.Services;

public interface INotificationService
{
    Task EnvoyerAsync(Guid destinataireId, string sujet, string contenu, CancellationToken cancellationToken = default);
}
