using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class MandatRepository : IMandatRepository
{
    private readonly AppDbContext _context;

    public MandatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Mandat?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Mandats.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Mandat>> GetByMandantAsync(Guid mandantId, CancellationToken cancellationToken = default)
        => await _context.Mandats
            .Where(m => m.MandantId == mandantId)
            .OrderByDescending(m => m.DateDebut)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Mandat>> GetByMandataireAsync(Guid mandataireId, CancellationToken cancellationToken = default)
        => await _context.Mandats
            .Where(m => m.MandataireId == mandataireId)
            .OrderByDescending(m => m.DateDebut)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Mandat mandat, CancellationToken cancellationToken = default)
    {
        await _context.Mandats.AddAsync(mandat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Mandat mandat, CancellationToken cancellationToken = default)
    {
        _context.Mandats.Update(mandat);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
