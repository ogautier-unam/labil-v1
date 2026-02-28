using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrisisConnect.Infrastructure.Persistence.Repositories;

public class PersonneRepository : IPersonneRepository
{
    private readonly AppDbContext _context;

    public PersonneRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Personne?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Personnes.FindAsync([id], cancellationToken);

    public async Task<Personne?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Personnes.FirstOrDefaultAsync(p => p.Email == email, cancellationToken);

    public async Task AddAsync(Personne personne, CancellationToken cancellationToken = default)
    {
        await _context.Personnes.AddAsync(personne, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Personne personne, CancellationToken cancellationToken = default)
    {
        _context.Personnes.Update(personne);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
