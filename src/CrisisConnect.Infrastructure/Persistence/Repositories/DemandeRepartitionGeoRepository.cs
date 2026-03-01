using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class DemandeRepartitionGeoRepository : IDemandeRepartitionGeoRepository
{
    private readonly AppDbContext _context;

    public DemandeRepartitionGeoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DemandeRepartitionGeo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Propositions.OfType<DemandeRepartitionGeo>()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    public async Task<IReadOnlyList<DemandeRepartitionGeo>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Propositions.OfType<DemandeRepartitionGeo>()
            .ToListAsync(cancellationToken);

    public async Task AddAsync(DemandeRepartitionGeo demande, CancellationToken cancellationToken = default)
    {
        await _context.Propositions.AddAsync(demande, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(DemandeRepartitionGeo demande, CancellationToken cancellationToken = default)
    {
        _context.Propositions.Update(demande);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
