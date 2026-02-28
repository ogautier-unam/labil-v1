using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IOffreRepository
{
    Task<Offre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Offre>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Offre offre, CancellationToken cancellationToken = default);
    Task UpdateAsync(Offre offre, CancellationToken cancellationToken = default);
}
