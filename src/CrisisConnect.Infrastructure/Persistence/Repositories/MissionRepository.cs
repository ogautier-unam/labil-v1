using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class MissionRepository : IMissionRepository
{
    private readonly AppDbContext _context;

    public MissionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Mission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Missions.Include(m => m.Matchings).FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Mission>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Missions.ToListAsync(cancellationToken);

    public async Task AddAsync(Mission mission, CancellationToken cancellationToken = default)
    {
        await _context.Missions.AddAsync(mission, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Mission mission, CancellationToken cancellationToken = default)
    {
        _context.Missions.Update(mission);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
