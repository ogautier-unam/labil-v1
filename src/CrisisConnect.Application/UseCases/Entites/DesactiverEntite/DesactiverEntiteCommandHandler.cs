using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Entites.DesactiverEntite;

public class DesactiverEntiteCommandHandler : ICommandHandler<DesactiverEntiteCommand>
{
    private readonly IEntiteRepository _repository;

    public DesactiverEntiteCommandHandler(IEntiteRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<Unit> Handle(DesactiverEntiteCommand request, CancellationToken cancellationToken)
    {
        var entite = await _repository.GetByIdAsync(request.EntiteId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Entite), request.EntiteId);

        entite.Desactiver();
        await _repository.UpdateAsync(entite, cancellationToken);
        return Unit.Value;
    }
}
