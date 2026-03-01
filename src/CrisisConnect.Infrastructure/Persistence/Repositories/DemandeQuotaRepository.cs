using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class DemandeQuotaRepository : IDemandeQuotaRepository
{
    private readonly AppDbContext _context;

    public DemandeQuotaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DemandeQuota?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Propositions.OfType<DemandeQuota>()
            .Include(d => d.Intentions)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    public async Task<IReadOnlyList<DemandeQuota>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Propositions.OfType<DemandeQuota>()
            .Include(d => d.Intentions)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(DemandeQuota demande, CancellationToken cancellationToken = default)
    {
        await _context.Propositions.AddAsync(demande, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(DemandeQuota demande, CancellationToken cancellationToken = default)
    {
        _context.Propositions.Update(demande);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
