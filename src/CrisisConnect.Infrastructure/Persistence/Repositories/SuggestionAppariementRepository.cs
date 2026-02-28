using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class SuggestionAppariementRepository : ISuggestionAppariementRepository
{
    private readonly AppDbContext _context;

    public SuggestionAppariementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SuggestionAppariement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.SuggestionsAppariement.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<SuggestionAppariement>> GetByOffreAsync(Guid offreId, CancellationToken cancellationToken = default)
        => await _context.SuggestionsAppariement
            .Where(s => s.OffreId == offreId)
            .OrderByDescending(s => s.ScoreCorrespondance)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<SuggestionAppariement>> GetByDemandeAsync(Guid demandeId, CancellationToken cancellationToken = default)
        => await _context.SuggestionsAppariement
            .Where(s => s.DemandeId == demandeId)
            .OrderByDescending(s => s.ScoreCorrespondance)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<SuggestionAppariement>> GetNonAcknowledgedAsync(CancellationToken cancellationToken = default)
        => await _context.SuggestionsAppariement
            .Where(s => !s.EstAcknowledged)
            .OrderByDescending(s => s.ScoreCorrespondance)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(SuggestionAppariement suggestion, CancellationToken cancellationToken = default)
    {
        await _context.SuggestionsAppariement.AddAsync(suggestion, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SuggestionAppariement suggestion, CancellationToken cancellationToken = default)
    {
        _context.SuggestionsAppariement.Update(suggestion);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
