using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IDemandeSurCatalogueRepository
{
    Task<DemandeSurCatalogue?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DemandeSurCatalogue>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(DemandeSurCatalogue demande, CancellationToken cancellationToken = default);
    Task UpdateAsync(DemandeSurCatalogue demande, CancellationToken cancellationToken = default);
}
