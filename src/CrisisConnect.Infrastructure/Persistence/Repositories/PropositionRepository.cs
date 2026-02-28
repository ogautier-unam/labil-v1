using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class PropositionRepository : IPropositionRepository
{
    private readonly AppDbContext _context;

    public PropositionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Proposition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Propositions.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<Proposition>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Propositions.ToListAsync(cancellationToken);

    public async Task AddAsync(Proposition proposition, CancellationToken cancellationToken = default)
    {
        await _context.Propositions.AddAsync(proposition, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Proposition proposition, CancellationToken cancellationToken = default)
    {
        _context.Propositions.Update(proposition);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Proposition proposition, CancellationToken cancellationToken = default)
    {
        _context.Propositions.Remove(proposition);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
