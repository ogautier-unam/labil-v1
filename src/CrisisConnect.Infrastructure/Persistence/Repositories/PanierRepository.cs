using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class PanierRepository : IPanierRepository
{
    private readonly AppDbContext _context;

    public PanierRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Panier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Paniers
            .Include(p => p.Offres)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Panier>> GetByProprietaireAsync(Guid proprietaireId, CancellationToken cancellationToken = default)
        => await _context.Paniers
            .Include(p => p.Offres)
            .Where(p => p.ProprietaireId == proprietaireId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Panier panier, CancellationToken cancellationToken = default)
    {
        await _context.Paniers.AddAsync(panier, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Panier panier, CancellationToken cancellationToken = default)
    {
        _context.Paniers.Update(panier);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
