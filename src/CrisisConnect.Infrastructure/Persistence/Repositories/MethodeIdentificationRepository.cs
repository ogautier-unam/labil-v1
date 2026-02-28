using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class MethodeIdentificationRepository : IMethodeIdentificationRepository
{
    private readonly AppDbContext _context;

    public MethodeIdentificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MethodeIdentification?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.MethodesIdentification.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<MethodeIdentification>> GetByPersonneAsync(Guid personneId, CancellationToken cancellationToken = default)
        => await _context.MethodesIdentification
            .Where(m => m.PersonneId == personneId)
            .OrderByDescending(m => m.NiveauFiabilite)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(MethodeIdentification methode, CancellationToken cancellationToken = default)
    {
        await _context.MethodesIdentification.AddAsync(methode, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(MethodeIdentification methode, CancellationToken cancellationToken = default)
    {
        _context.MethodesIdentification.Update(methode);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
