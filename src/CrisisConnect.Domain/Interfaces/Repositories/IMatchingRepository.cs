using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IMatchingRepository
{
    Task<Matching?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Matching>> GetByMissionIdAsync(Guid missionId, CancellationToken cancellationToken = default);
    Task AddAsync(Matching matching, CancellationToken cancellationToken = default);
    Task UpdateAsync(Matching matching, CancellationToken cancellationToken = default);
}
