using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IMediaRepository
{
    Task<IReadOnlyList<Media>> GetByPropositionIdAsync(Guid propositionId, CancellationToken cancellationToken = default);
    Task AddAsync(Media media, CancellationToken cancellationToken = default);
    Task<Media?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteAsync(Media media, CancellationToken cancellationToken = default);
}
