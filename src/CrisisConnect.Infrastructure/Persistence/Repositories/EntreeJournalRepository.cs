using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class EntreeJournalRepository : IEntreeJournalRepository
{
    private readonly AppDbContext _context;

    public EntreeJournalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(EntreeJournal entree, CancellationToken cancellationToken = default)
    {
        await _context.EntreesJournal.AddAsync(entree, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<EntreeJournal>> GetByActeurAsync(Guid acteurId, CancellationToken cancellationToken = default)
        => await _context.EntreesJournal
            .Where(e => e.ActeurId == acteurId)
            .OrderByDescending(e => e.DateHeure)
            .ToListAsync(cancellationToken);
}
