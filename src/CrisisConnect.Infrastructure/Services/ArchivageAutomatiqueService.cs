using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrisisConnect.Infrastructure.Services;

/// <summary>
/// Service hébergé qui archive automatiquement les propositions inactives
/// selon les délais configurés dans ConfigCatastrophe (§5.1 ex.1).
///
/// Cycle toutes les heures :
///   1. Propositions Active depuis plus de (DelaiArchivage − DelaiRappel) jours
///      → MarquerEnAttenteRelance + notification TypeNotification.RelancePropositionAvantArchivage
///   2. Propositions EnAttenteRelance depuis plus de DelaiRappelAvantArchivage jours
///      → Archiver()
/// </summary>
public class ArchivageAutomatiqueService : BackgroundService
{
    private static readonly TimeSpan Periode = TimeSpan.FromHours(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ArchivageAutomatiqueService> _logger;

    public ArchivageAutomatiqueService(IServiceScopeFactory scopeFactory, ILogger<ArchivageAutomatiqueService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ArchivageAutomatiqueService démarré.");

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
            var configRepo = scope.ServiceProvider.GetRequiredService<IConfigCatastropheRepository>();
            var propositionRepo = scope.ServiceProvider.GetRequiredService<IPropositionRepository>();
            var notifRepo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
            var notifService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var config = await configRepo.GetActiveAsync(ct);
            if (config is null)
            {
                _logger.LogDebug("Aucune configuration catastrophe active — cycle archivage ignoré.");
                return;
            }

            var maintenant = DateTime.UtcNow;
            var seuilRappel = maintenant.AddDays(-(config.DelaiArchivageJours - config.DelaiRappelAvantArchivage));
            var seuilArchivage = maintenant.AddDays(-config.DelaiArchivageJours);

            var propositions = await propositionRepo.GetAllAsync(ct);

            int nbRappels = 0, nbArchivages = 0;

            foreach (var proposition in propositions)
            {
                var derniereActivite = proposition.ModifieLe ?? proposition.CreeLe;

                // Étape 1 — Relance imminente
                if (proposition.Statut == StatutProposition.Active && derniereActivite < seuilRappel)
                {
                    try
                    {
                        proposition.MarquerEnAttenteRelance();
                        await propositionRepo.UpdateAsync(proposition, ct);

                        var notification = new Notification(
                            proposition.CreePar,
                            TypeNotification.RelancePropositionAvantArchivage,
                            $"Votre proposition « {proposition.Titre} » sera archivée dans {config.DelaiRappelAvantArchivage} jour(s) si aucune activité n'est détectée.",
                            refEntiteId: proposition.Id.ToString());
                        await notifRepo.AddAsync(notification, ct);
                        await notifService.EnvoyerAsync(proposition.CreePar, "Archivage imminent", notification.Contenu, ct);
                        nbRappels++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Impossible de marquer en relance la proposition {Id}", proposition.Id);
                    }
                }

                // Étape 2 — Archivage automatique
                else if (proposition.Statut == StatutProposition.EnAttenteRelance
                    && proposition.DateRelance.HasValue
                    && proposition.DateRelance.Value < seuilArchivage)
                {
                    try
                    {
                        proposition.Archiver();
                        await propositionRepo.UpdateAsync(proposition, ct);
                        nbArchivages++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Impossible d'archiver la proposition {Id}", proposition.Id);
                    }
                }
            }

            if (nbRappels > 0 || nbArchivages > 0)
                _logger.LogInformation(
                    "Cycle archivage : {Rappels} rappel(s) envoyé(s), {Archivages} proposition(s) archivée(s).",
                    nbRappels, nbArchivages);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Erreur inattendue dans le cycle ArchivageAutomatiqueService.");
        }
    }
}
