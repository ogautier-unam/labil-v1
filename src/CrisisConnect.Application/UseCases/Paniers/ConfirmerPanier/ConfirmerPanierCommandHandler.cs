using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Paniers.ConfirmerPanier;

public class ConfirmerPanierCommandHandler : ICommandHandler<ConfirmerPanierCommand>
{
    private readonly IPanierRepository _panierRepository;

    public ConfirmerPanierCommandHandler(IPanierRepository panierRepository)
    {
        _panierRepository = panierRepository;
    }

    public async ValueTask<Unit> Handle(ConfirmerPanierCommand request, CancellationToken cancellationToken)
    {
        var panier = await _panierRepository.GetByIdAsync(request.PanierId, cancellationToken)
            ?? throw new NotFoundException("Panier", request.PanierId);

        panier.Confirmer();
        await _panierRepository.UpdateAsync(panier, cancellationToken);
        return Unit.Value;
    }
}
