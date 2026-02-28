using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class CategorieTaxonomieRepository : ICategorieTaxonomieRepository
{
    private readonly AppDbContext _context;

    public CategorieTaxonomieRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CategorieTaxonomie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.CategoriesTaxonomie
            .Include(c => c.SousCategories)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<IReadOnlyList<CategorieTaxonomie>> GetRacinesAsync(Guid configId, CancellationToken cancellationToken = default)
        => await _context.CategoriesTaxonomie
            .Where(c => c.ConfigId == configId && c.ParentId == null && c.EstActive)
            .Include(c => c.SousCategories)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CategorieTaxonomie>> GetSousCategoriesAsync(Guid parentId, CancellationToken cancellationToken = default)
        => await _context.CategoriesTaxonomie
            .Where(c => c.ParentId == parentId && c.EstActive)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(CategorieTaxonomie categorie, CancellationToken cancellationToken = default)
    {
        await _context.CategoriesTaxonomie.AddAsync(categorie, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(CategorieTaxonomie categorie, CancellationToken cancellationToken = default)
    {
        _context.CategoriesTaxonomie.Update(categorie);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
