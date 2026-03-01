using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly AppDbContext _context;

    public MediaRepository(AppDbContext context) => _context = context;

    public async Task<IReadOnlyList<Media>> GetByPropositionIdAsync(Guid propositionId, CancellationToken cancellationToken = default)
        => await _context.Medias
            .Where(m => m.PropositionId == propositionId)
            .OrderByDescending(m => m.DateAjout)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Media media, CancellationToken cancellationToken = default)
    {
        await _context.Medias.AddAsync(media, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Media?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Medias.FindAsync([id], cancellationToken);

    public async Task DeleteAsync(Media media, CancellationToken cancellationToken = default)
    {
        _context.Medias.Remove(media);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
