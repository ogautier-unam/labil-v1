using System.Reflection;
using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Acteurs (TPH)
    public DbSet<Proposition> Propositions => Set<Proposition>();
    public DbSet<Personne> Personnes => Set<Personne>();
    public DbSet<Entite> Entites => Set<Entite>();

    // Transactions & discussions
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Discussion> Discussions => Set<Discussion>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Panier> Paniers => Set<Panier>();

    // Rôles & mandats
    public DbSet<AttributionRole> AttributionsRoles => Set<AttributionRole>();
    public DbSet<Mandat> Mandats => Set<Mandat>();

    // Journal & notifications
    public DbSet<EntreeJournal> EntreesJournal => Set<EntreeJournal>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    // P3 — Config & taxonomie
    public DbSet<ConfigCatastrophe> ConfigsCatastrophe => Set<ConfigCatastrophe>();
    public DbSet<CategorieTaxonomie> CategoriesTaxonomie => Set<CategorieTaxonomie>();

    // P4 — Médias & lignes catalogue & intentions de don
    public DbSet<Media> Medias => Set<Media>();
    public DbSet<IntentionDon> IntentionsDon => Set<IntentionDon>();
    public DbSet<LigneCatalogue> LignesCatalogue => Set<LigneCatalogue>();

    // P2 — Méthodes d'identification (TPH)
    public DbSet<MethodeIdentification> MethodesIdentification => Set<MethodeIdentification>();

    // P6 — Suggestions d'appariement
    public DbSet<SuggestionAppariement> SuggestionsAppariement => Set<SuggestionAppariement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
