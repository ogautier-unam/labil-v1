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

        // Repositories — P2 & P6
        services.AddScoped<IMethodeIdentificationRepository, MethodeIdentificationRepository>();
        services.AddScoped<ISuggestionAppariementRepository, SuggestionAppariementRepository>();

        // Services d'authentification
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // P5 — Adapter traduction (corpus interne par défaut)
        services.AddScoped<IServiceTraduction, AdaptateurCorpusInterne>();

        // P5 — Stratégies de priorisation (enregistrées individuellement pour injection nommée)
        services.AddScoped<PriorisationParAnciennete>();
        services.AddScoped<PriorisationParUrgence>();
        services.AddScoped<PriorisationParRegionSeverite>();
        services.AddScoped<PriorisationParType>();

        // HTTP clients pour les adaptateurs de traduction externes
        services.AddHttpClient<AdaptateurDeepL>();
        services.AddHttpClient<AdaptateurLibreTranslate>();

        return services;
    }
}
