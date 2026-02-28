using System.Reflection;
using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Proposition> Propositions => Set<Proposition>();
    public DbSet<Personne> Personnes => Set<Personne>();
    public DbSet<Mission> Missions => Set<Mission>();
    public DbSet<Matching> Matchings => Set<Matching>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
