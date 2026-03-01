using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>
/// Service hébergé qui envoie un rappel aux acteurs dont le rôle expire bientôt (L11).
///
/// Cycle toutes les heures : récupère les rôles actifs avec DateRappel &lt;= now et
/// !RappelEnvoye → envoie une notification + MarquerRappelEnvoye() + UpdateAsync.
/// </summary>
public class RappelExpirationRoleService : BackgroundService
{
    private static readonly TimeSpan Periode = TimeSpan.FromHours(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RappelExpirationRoleService> _logger;

    public RappelExpirationRoleService(IServiceScopeFactory scopeFactory, ILogger<RappelExpirationRoleService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RappelExpirationRoleService démarré.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunCycleAsync(stoppingToken);
            await Task.Delay(Periode, stoppingToken).ConfigureAwait(false);
        }
    }

    private async Task RunCycleAsync(CancellationToken ct)
    {
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var roleRepo = scope.ServiceProvider.GetRequiredService<IAttributionRoleRepository>();
            var notifRepo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
            var notifService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var rappelsDus = await roleRepo.GetRappelsDusAsync(ct);
            int nbEnvoyes = 0;

            foreach (var role in rappelsDus)
            {
                try
                {
                    var joursRestants = role.DateFin.HasValue
                        ? (int)Math.Ceiling((role.DateFin.Value - DateTime.UtcNow).TotalDays)
                        : 0;

                    var contenu = $"Votre rôle {role.TypeRole} expire dans {joursRestants} jour(s).";

                    var notification = new Notification(
                        role.ActeurId,
                        TypeNotification.RappelRoleExpirationImminente,
                        contenu,
                        refEntiteId: role.Id.ToString());

                    await notifRepo.AddAsync(notification, ct);
                    await notifService.EnvoyerAsync(role.ActeurId, "Expiration de rôle imminente", contenu, ct);

                    role.MarquerRappelEnvoye();
                    await roleRepo.UpdateAsync(role, ct);
                    nbEnvoyes++;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Impossible d'envoyer le rappel pour le rôle {Id}", role.Id);
                }
            }

            if (nbEnvoyes > 0)
                _logger.LogInformation("Cycle rappel rôles : {Nb} rappel(s) envoyé(s).", nbEnvoyes);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Erreur inattendue dans le cycle RappelExpirationRoleService.");
        }
    }
}
