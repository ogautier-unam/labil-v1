using CrisisConnect.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Tests;

/// <summary>
/// Crée un AppDbContext isolé en mémoire (InMemory) pour les tests d'infrastructure.
/// Chaque appel génère une base avec un nom unique pour garantir l'isolation.
/// </summary>
public static class DbContextFactory
{
    public static AppDbContext Créer()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
