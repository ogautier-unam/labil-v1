using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IAttributionRoleRepository
{
    Task<AttributionRole?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AttributionRole>> GetByActeurAsync(Guid acteurId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AttributionRole>> GetByTypeRoleAsync(TypeRole typeRole, CancellationToken cancellationToken = default);
    Task AddAsync(AttributionRole attribution, CancellationToken cancellationToken = default);
    Task UpdateAsync(AttributionRole attribution, CancellationToken cancellationToken = default);
}
