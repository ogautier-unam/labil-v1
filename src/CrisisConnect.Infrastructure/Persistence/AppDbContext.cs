using System.Reflection;
using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Proposition> Propositions => Set<Proposition>();
    public DbSet<Personne> Personnes => Set<Personne>();
    public DbSet<Entite> Entites => Set<Entite>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Discussion> Discussions => Set<Discussion>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Panier> Paniers => Set<Panier>();
    public DbSet<AttributionRole> AttributionsRoles => Set<AttributionRole>();
    public DbSet<Mandat> Mandats => Set<Mandat>();
    public DbSet<EntreeJournal> EntreesJournal => Set<EntreeJournal>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
