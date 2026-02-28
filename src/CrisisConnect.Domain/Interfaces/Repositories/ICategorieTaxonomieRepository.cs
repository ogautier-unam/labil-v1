using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface ICategorieTaxonomieRepository
{
    Task<CategorieTaxonomie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CategorieTaxonomie>> GetRacinesAsync(Guid configId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CategorieTaxonomie>> GetSousCategoriesAsync(Guid parentId, CancellationToken cancellationToken = default);
    Task AddAsync(CategorieTaxonomie categorie, CancellationToken cancellationToken = default);
    Task UpdateAsync(CategorieTaxonomie categorie, CancellationToken cancellationToken = default);
}
