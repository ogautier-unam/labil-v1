using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using CrisisConnect.Infrastructure.Persistence;
using CrisisConnect.Infrastructure.Persistence.Repositories;
using CrisisConnect.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CrisisConnect.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default"))
                   .UseSnakeCaseNamingConvention());

        // Repositories — Propositions
        services.AddScoped<IPropositionRepository, PropositionRepository>();
        services.AddScoped<IOffreRepository, OffreRepository>();
        services.AddScoped<IDemandeRepository, DemandeRepository>();

        // Repositories — Acteurs
        services.AddScoped<IPersonneRepository, PersonneRepository>();
        services.AddScoped<IEntiteRepository, EntiteRepository>();
        services.AddScoped<IAttributionRoleRepository, AttributionRoleRepository>();
        services.AddScoped<IMandatRepository, MandatRepository>();

        // Repositories — Transactions & paniers
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IPanierRepository, PanierRepository>();

        // Repositories — Journal & notifications
        services.AddScoped<IEntreeJournalRepository, EntreeJournalRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        // Repositories — P3
        services.AddScoped<IConfigCatastropheRepository, ConfigCatastropheRepository>();
        services.AddScoped<ICategorieTaxonomieRepository, CategorieTaxonomieRepository>();

        // Repositories — P2, P4 & P6
        services.AddScoped<IMethodeIdentificationRepository, MethodeIdentificationRepository>();
        services.AddScoped<ISuggestionAppariementRepository, SuggestionAppariementRepository>();
        services.AddScoped<IDemandeQuotaRepository, DemandeQuotaRepository>();
        services.AddScoped<IDemandeSurCatalogueRepository, DemandeSurCatalogueRepository>();
        services.AddScoped<IDemandeRepartitionGeoRepository, DemandeRepartitionGeoRepository>();
        services.AddScoped<IMediaRepository, MediaRepository>();

        // Services d'authentification
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<INotificationService, NotificationService>();

        // P5 — Adapter traduction (corpus interne par défaut)
        services.AddScoped<IServiceTraduction, AdaptateurCorpusInterne>();

        // P5 — Stratégies de priorisation (NF-11 : injectables via IEnumerable<IStrategiePriorisation>)
        services.AddScoped<IStrategiePriorisation, PriorisationParAnciennete>();
        services.AddScoped<IStrategiePriorisation, PriorisationParUrgence>();
        services.AddScoped<IStrategiePriorisation, PriorisationParRegionSeverite>();
        services.AddScoped<IStrategiePriorisation, PriorisationParType>();

        // HTTP clients pour les adaptateurs de traduction externes
        services.AddHttpClient<AdaptateurDeepL>();
        services.AddHttpClient<AdaptateurLibreTranslate>();

        // Background services
        services.AddHostedService<ArchivageAutomatiqueService>();     // §5.1 ex.1 — archivage automatique
        services.AddHostedService<RappelExpirationRoleService>();     // L11 — rappel expiration rôle

        return services;
    }
}
