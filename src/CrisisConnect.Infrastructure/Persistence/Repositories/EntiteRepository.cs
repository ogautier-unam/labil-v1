using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class EntiteRepository : IEntiteRepository
{
    private readonly AppDbContext _context;

    public EntiteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Entite?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Entites.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<Entite>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Entites.ToListAsync(cancellationToken);

    public async Task AddAsync(Entite entite, CancellationToken cancellationToken = default)
    {
        await _context.Entites.AddAsync(entite, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Entite entite, CancellationToken cancellationToken = default)
    {
        _context.Entites.Update(entite);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
