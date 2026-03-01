using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class DemandeSurCatalogueRepository : IDemandeSurCatalogueRepository
{
    private readonly AppDbContext _context;

    public DemandeSurCatalogueRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DemandeSurCatalogue?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Propositions.OfType<DemandeSurCatalogue>()
            .Include(d => d.Lignes)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    public async Task<IReadOnlyList<DemandeSurCatalogue>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Propositions.OfType<DemandeSurCatalogue>()
            .Include(d => d.Lignes)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(DemandeSurCatalogue demande, CancellationToken cancellationToken = default)
    {
        await _context.Propositions.AddAsync(demande, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(DemandeSurCatalogue demande, CancellationToken cancellationToken = default)
    {
        _context.Propositions.Update(demande);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
