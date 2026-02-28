using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class DemandeRepository : IDemandeRepository
{
    private readonly AppDbContext _context;

    public DemandeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Demande?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Propositions.OfType<Demande>().FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Demande>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Propositions.OfType<Demande>().ToListAsync(cancellationToken);

    public async Task AddAsync(Demande demande, CancellationToken cancellationToken = default)
    {
        await _context.Propositions.AddAsync(demande, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Demande demande, CancellationToken cancellationToken = default)
    {
        _context.Propositions.Update(demande);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
