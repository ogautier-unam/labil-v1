using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class OffreRepository : IOffreRepository
{
    private readonly AppDbContext _context;

    public OffreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Offre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Propositions.OfType<Offre>()
            .Include(o => o.DemandesCouplees)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Offre>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Propositions.OfType<Offre>()
            .Include(o => o.DemandesCouplees)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Offre offre, CancellationToken cancellationToken = default)
    {
        await _context.Propositions.AddAsync(offre, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Offre offre, CancellationToken cancellationToken = default)
    {
        _context.Propositions.Update(offre);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
