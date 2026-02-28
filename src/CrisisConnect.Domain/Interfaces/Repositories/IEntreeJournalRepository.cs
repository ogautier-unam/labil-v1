using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IEntreeJournalRepository
{
    Task AddAsync(EntreeJournal entree, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EntreeJournal>> GetByActeurAsync(Guid acteurId, CancellationToken cancellationToken = default);
}
