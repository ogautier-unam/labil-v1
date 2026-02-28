using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class ConfigCatastropheRepository : IConfigCatastropheRepository
{
    private readonly AppDbContext _context;

    public ConfigCatastropheRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ConfigCatastrophe?> GetActiveAsync(CancellationToken cancellationToken = default)
        => await _context.ConfigsCatastrophe
            .FirstOrDefaultAsync(c => c.EstActive, cancellationToken);

    public async Task<ConfigCatastrophe?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.ConfigsCatastrophe.FindAsync([id], cancellationToken);

    public async Task AddAsync(ConfigCatastrophe config, CancellationToken cancellationToken = default)
    {
        await _context.ConfigsCatastrophe.AddAsync(config, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ConfigCatastrophe config, CancellationToken cancellationToken = default)
    {
        _context.ConfigsCatastrophe.Update(config);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
