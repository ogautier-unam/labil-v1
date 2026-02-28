using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class MatchingRepository : IMatchingRepository
{
    private readonly AppDbContext _context;

    public MatchingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Matching?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Matchings.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<Matching>> GetByMissionIdAsync(Guid missionId, CancellationToken cancellationToken = default)
        => await _context.Matchings.Where(m => m.MissionId == missionId).ToListAsync(cancellationToken);

    public async Task AddAsync(Matching matching, CancellationToken cancellationToken = default)
    {
        await _context.Matchings.AddAsync(matching, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Matching matching, CancellationToken cancellationToken = default)
    {
        _context.Matchings.Update(matching);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
