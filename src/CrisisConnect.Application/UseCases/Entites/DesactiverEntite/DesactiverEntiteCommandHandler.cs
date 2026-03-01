using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Entites.DesactiverEntite;

public class DesactiverEntiteCommandHandler : IRequestHandler<DesactiverEntiteCommand>
{
    private readonly IEntiteRepository _repository;

    public DesactiverEntiteCommandHandler(IEntiteRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DesactiverEntiteCommand request, CancellationToken cancellationToken)
    {
        var entite = await _repository.GetByIdAsync(request.EntiteId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Entite), request.EntiteId);

        entite.Desactiver();
        await _repository.UpdateAsync(entite, cancellationToken);
    }
}
