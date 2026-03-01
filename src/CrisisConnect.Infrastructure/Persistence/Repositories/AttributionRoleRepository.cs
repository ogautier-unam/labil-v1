using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class AttributionRoleRepository : IAttributionRoleRepository
{
    private readonly AppDbContext _context;

    public AttributionRoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AttributionRole?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.AttributionsRoles.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<IReadOnlyList<AttributionRole>> GetByActeurAsync(Guid acteurId, CancellationToken cancellationToken = default)
        => await _context.AttributionsRoles
            .Where(a => a.ActeurId == acteurId)
            .OrderByDescending(a => a.DateDebut)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<AttributionRole>> GetByTypeRoleAsync(TypeRole typeRole, CancellationToken cancellationToken = default)
        => await _context.AttributionsRoles
            .Where(a => a.TypeRole == typeRole && a.Statut == StatutRole.Actif)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(AttributionRole attribution, CancellationToken cancellationToken = default)
    {
        await _context.AttributionsRoles.AddAsync(attribution, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AttributionRole attribution, CancellationToken cancellationToken = default)
    {
        _context.AttributionsRoles.Update(attribution);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
