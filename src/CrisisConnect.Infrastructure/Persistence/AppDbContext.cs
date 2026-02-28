using System.Reflection;
using CrisisConnect.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Proposition> Propositions => Set<Proposition>();
    public DbSet<Personne> Personnes => Set<Personne>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
